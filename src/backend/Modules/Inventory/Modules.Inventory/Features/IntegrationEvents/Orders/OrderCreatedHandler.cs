using EShop.Contracts.CQ.Orders.Events;
using EShop.Contracts.Events.Inventory;
using Microsoft.EntityFrameworkCore;
using Modules.Inventory.Infrastructure.Persistence.Database.Context;
using Wolverine;
using Wolverine.Attributes;

namespace Modules.Inventory.Features.IntegrationEvents.Orders;

[Transactional(typeof(InventoryDbContext))]
public sealed class OrderCreatedHandler(
    InventoryDbContext context,
    IMessageBus bus)
{
    public async Task Handle(
        OrderCreatedEvent @event,
        CancellationToken ct)
    {
        var productIds = @event.Items
            .Select(x => x.ProductId)
            .ToArray();

        var stocks = await context.Stocks
            .Where(x => productIds.Contains(x.ProductId))
            .ToDictionaryAsync(x => x.ProductId, ct);

        foreach (var item in @event.Items)
        {
            if (!stocks.TryGetValue(item.ProductId, out var stock))
            {
                await bus.PublishAsync(
                    new InventoryReservationFailedEvent(
                        @event.OrderId,
                        $"Stock not found for product {item.ProductId}"));

                return;
            }

            var available = stock.Quantity - stock.ReservedQuantity;

            if (available < item.Quantity)
            {
                await bus.PublishAsync(
                    new InventoryReservationFailedEvent(
                        @event.OrderId,
                        $"Not enough stock for product {item.ProductId}"));

                return;
            }
        }

        foreach (var item in @event.Items)
        {
            var stock = stocks[item.ProductId];

            stock.ReservedQuantity += item.Quantity;
            stock.UpdatedAt = DateTime.UtcNow;
        }

        await bus.PublishAsync(
            new InventoryReservedEvent(
                @event.OrderId));
    }
}