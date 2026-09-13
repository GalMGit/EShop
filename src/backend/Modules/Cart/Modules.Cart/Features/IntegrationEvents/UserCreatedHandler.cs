using EShop.Contracts.Events.Identity.Events;
using Microsoft.EntityFrameworkCore;
using Modules.Cart.Domain;
using Modules.Cart.Infrastructure.Persistence.Database.Context;

namespace Modules.Cart.Features.IntegrationEvents;

public sealed class UserCreatedHandler(
    CartDbContext context)
{
    public async Task Handle(
        UserCreatedEvent @event,
        CancellationToken ct)
    {
        var cartExists = await context.Carts
            .AnyAsync(x => 
                x.UserId == @event.UserId, ct);

        if (!cartExists)
        {
            var cart = new ShopCart
            {
                Id = Guid.CreateVersion7(),
                UserId = @event.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await context.Carts.AddAsync(cart, ct);
            await context.SaveChangesAsync(ct);
        }
    }
}