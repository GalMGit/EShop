using EShop.Contracts.CQ.Cart;
using EShop.Contracts.CQ.Cart.Responses;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Cart.Errors;
using Modules.Cart.Infrastructure.Persistence.Database.Context;

namespace Modules.Cart.Features.GetCartForCheckout;

public sealed class GetCartForCheckoutHandler(
    CartDbContext context)
{
    public async Task<Result<CartForCheckoutResponse>> Handle(
        GetCartForCheckoutQuery query, 
        CancellationToken ct)
    {
        var cart = await context.Carts
            .AsNoTracking()
            .SingleOrDefaultAsync(x => 
                x.UserId == query.UserId, ct);

        if (cart is null)
            return Result<CartForCheckoutResponse>.Failure(
                CartErrors.NotFound);

        var items = await context.CartItems
            .AsNoTracking()
            .Where(x => x.CartId == cart.Id)
            .Select(x => new CartItemForCheckoutResponse(
                x.ProductId,
                x.Quantity))
            .ToListAsync(ct);

        return Result<CartForCheckoutResponse>.Success(
            new CartForCheckoutResponse(items));
    }
}