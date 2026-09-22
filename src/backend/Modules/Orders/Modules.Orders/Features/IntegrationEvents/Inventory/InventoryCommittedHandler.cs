using EShop.Contracts.CQ.Cart.Commands;
using EShop.Contracts.Events.Inventory;
using Microsoft.EntityFrameworkCore;
using Modules.Orders.Domain;
using Modules.Orders.Infrastructure.Persistence.Database.Context;
using Wolverine;
using Wolverine.Attributes;

namespace Modules.Orders.Features.IntegrationEvents.Inventory;

[Transactional(typeof(OrderDbContext))]
public sealed class InventoryCommittedHandler(
    OrderDbContext context,
    IMessageBus bus)
{
    public async Task Handle(
        InventoryCommittedEvent @event,
        CancellationToken ct)
    {
        var order = await context.Orders
            .SingleOrDefaultAsync(x => 
                x.Id == @event.OrderId, ct);
        
        if (order is null)
            return;

        if (order.Status != OrderStatus.Paid)
            return;

        order.Status = OrderStatus.Completed;

        await bus.SendAsync(
            new ClearCartCommand(
                order.UserId));
    }
}