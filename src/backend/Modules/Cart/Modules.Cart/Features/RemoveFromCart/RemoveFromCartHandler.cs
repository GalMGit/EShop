using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Cart.Errors;
using Modules.Cart.Infrastructure.Persistence.Database.Context;

namespace Modules.Cart.Features.RemoveFromCart;

public sealed class RemoveFromCartHandler(
    CartDbContext context)
{
    public async Task<Result> Handle(
        RemoveFromCartCommand command,
        CancellationToken ct)
    {
        var cart = await context.Carts
            .SingleOrDefaultAsync(x =>
                x.UserId == command.UserId, ct);
        
        if(cart is null)
            return Result.Failure(
                CartErrors.NotFound);

        var cartItem = await context.CartItems
            .SingleOrDefaultAsync(x => 
                x.CartId == cart.Id && 
                x.ProductId == command.ProductId, ct);

        if (cartItem is null)
            return Result.Failure(
                CartErrors.ItemNotFound);

        context.CartItems.Remove(cartItem);

        await context.SaveChangesAsync(ct);

        return Result.Success();
    }
}