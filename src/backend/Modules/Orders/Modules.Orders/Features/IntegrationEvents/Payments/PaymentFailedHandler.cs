using EShop.Contracts.Events.Payments;
using Microsoft.EntityFrameworkCore;
using Modules.Orders.Domain;
using Modules.Orders.Infrastructure.Persistence.Database.Context;
using Wolverine;
using Wolverine.Attributes;

namespace Modules.Orders.Features.IntegrationEvents.Payments;

[Transactional(typeof(OrderDbContext))]
public sealed class PaymentFailedHandler(
    OrderDbContext context,
    IMessageBus bus)
{
    public async Task Handle(
        PaymentFailedEvent @event,
        CancellationToken ct)
    {
        var order = await context.Orders
            .SingleOrDefaultAsync(x => 
                x.Id == @event.OrderId, ct);

        if (order is null)
            return;

        if (order.Status != OrderStatus.InventoryReserved)
            return;

        order.Status = OrderStatus.Cancelled;
    }
}