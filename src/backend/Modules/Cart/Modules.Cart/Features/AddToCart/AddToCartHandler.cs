using EShop.Contracts.CQ.Catalog;
using EShop.Contracts.CQ.Inventory.Responses;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Cart.Domain;
using Modules.Cart.Errors;
using Modules.Cart.Infrastructure.Persistence.Database.Context;
using Wolverine;

namespace Modules.Cart.Features.AddToCart;

public sealed class AddToCartHandler(
    CartDbContext context,
    IMessageBus bus)
{
    public async Task<Result> Handle(
        AddToCartCommand command, 
        CancellationToken ct)
    {
        var cart = await context.Carts
            .SingleOrDefaultAsync(x =>
                x.UserId == command.UserId, ct);

        if (cart is null)
            return Result.Failure(
                CartErrors.NotFound);

        var productResult = await bus.InvokeAsync<
            Result<ProductForCartResponse>>(
                new GetProductForCartQuery(
                    command.Request.ProductId), ct);

        if (productResult.IsFailure)
            return Result.Failure(
                productResult.Error!);

        var cartItem = await context.CartItems
            .SingleOrDefaultAsync(x =>
                    x.CartId == cart.Id &&
                    x.ProductId == productResult.Value!.Id,
                ct);

        if (cartItem is null)
        {
            cartItem = new CartItem
            {
                Id = Guid.CreateVersion7(),
                AddedAt = DateTime.UtcNow,
                CartId = cart.Id,
                ProductId = productResult.Value!.Id,
                Quantity = command.Request.Quantity
            };

            await context.CartItems.AddAsync(cartItem, ct);
        }
        else
        {
            cartItem.Quantity += command.Request.Quantity;
        }

        await context.SaveChangesAsync(ct);

        return Result.Success();
    }
}