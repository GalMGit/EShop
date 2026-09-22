using EShop.Contracts.CQ.Cart.Commands;
using Microsoft.EntityFrameworkCore;
using Modules.Cart.Infrastructure.Persistence.Database.Context;
using Wolverine.Attributes;

namespace Modules.Cart.Features.ClearCart;

[Transactional(typeof(CartDbContext))]
public sealed class ClearCartHandler(
    CartDbContext context)
{
    public async Task Handle(
        ClearCartCommand command, 
        CancellationToken ct)
    {
        var cart = await context.Carts
            .Include(x => x.Items)
            .SingleOrDefaultAsync(x => 
                x.UserId == command.UserId, ct);

        cart?.Items.Clear();
    }
}