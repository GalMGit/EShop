using EShop.Contracts.Events.Inventory;
using Microsoft.EntityFrameworkCore;
using Modules.Orders.Domain;
using Modules.Orders.Infrastructure.Persistence.Database.Context;
using Wolverine.Attributes;

namespace Modules.Orders.Features.IntegrationEvents.Inventory;

[Transactional(typeof(OrderDbContext))]
public sealed class InventoryReservationFailedHandler(
    OrderDbContext context)
{
    public async Task Handle(
        InventoryReservationFailedEvent @event,
        CancellationToken ct)
    {
         var order = await context.Orders
            .SingleOrDefaultAsync(x => 
                    x.Id == @event.OrderId, ct);

         if (order is null)
             return;

         if (order.Status != OrderStatus.Pending)
             return;

         order.Status = OrderStatus.Cancelled;
    }
}