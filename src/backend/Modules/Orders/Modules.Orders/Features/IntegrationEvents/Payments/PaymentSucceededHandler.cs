using EShop.Contracts.CQ.Inventory.Commands;
using EShop.Contracts.Events.Payments;
using Microsoft.EntityFrameworkCore;
using Modules.Orders.Domain;
using Modules.Orders.Infrastructure.Persistence.Database.Context;
using Wolverine;
using Wolverine.Attributes;

namespace Modules.Orders.Features.IntegrationEvents.Payments;

[Transactional(typeof(OrderDbContext))]
public sealed class PaymentSucceededHandler(
    OrderDbContext context,
    IMessageBus bus)
{
    public async Task Handle(
        PaymentSucceededEvent @event,
        CancellationToken ct)
    {
        var order = await context.Orders
            .Include(x => x.Items)
            .SingleOrDefaultAsync(x => 
                x.Id == @event.OrderId, ct);
        
        if(order is null)
            return;

        if (order.Status != OrderStatus.InventoryReserved)
            return;

        order.Status = OrderStatus.Paid;

        await bus.SendAsync(
            new CommitInventoryCommand(
                order.Id,
                order.Items
                    .Select(x => new CommitInventoryItem(
                        x.ProductId,
                        x.Quantity))
                    .ToArray()));
    }
}