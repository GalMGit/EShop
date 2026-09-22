using EShop.Contracts.CQ.Inventory.Commands;
using EShop.Contracts.Events.Inventory;
using Microsoft.EntityFrameworkCore;
using Modules.Inventory.Infrastructure.Persistence.Database.Context;
using Wolverine;
using Wolverine.Attributes;

namespace Modules.Inventory.Features.CommitInventory;

[Transactional(typeof(InventoryDbContext))]
public sealed class CommitInventoryHandler(
    InventoryDbContext context,
    IMessageBus bus)
{
    public async Task Handle(
        CommitInventoryCommand command,
        CancellationToken ct)
    {
        var productIds = command.Items
            .Select(x => x.ProductId)
            .ToArray();

        var stocks = await context.Stocks
            .Where(x => productIds.Contains(x.ProductId))
            .ToDictionaryAsync(x => 
                x.ProductId, ct);
        
        foreach (var item in command.Items)
        {
            if (!stocks.TryGetValue(
                    item.ProductId,
                    out var stock))
            {
                throw new InvalidOperationException(
                    $"Stock not found: {item.ProductId}");
            }

            if (stock.ReservedQuantity < item.Quantity)
            {
                throw new InvalidOperationException(
                    $"Invalid reservation for product: {item.ProductId}");
            }
        }
        
        foreach (var item in command.Items)
        {
            var stock = stocks[item.ProductId];

            stock.Quantity -= item.Quantity;
            stock.ReservedQuantity -= item.Quantity;
            stock.UpdatedAt = DateTime.UtcNow;
        }

        await bus.PublishAsync(
            new InventoryCommittedEvent(
                command.OrderId));
    }
}