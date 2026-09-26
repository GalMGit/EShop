using EShop.Contracts.CQ.Catalog;
using EShop.Contracts.CQ.Catalog.Responses;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Cart.Application.DTOs;
using Modules.Cart.Errors;
using Modules.Cart.Infrastructure.Persistence.Database.Context;
using Wolverine;

namespace Modules.Cart.Features.GetCart;

public sealed class GetCartHandler(
    CartDbContext context,
    IMessageBus bus)
{
    public async Task<Result<CartResponse>> Handle(
        GetCartQuery query, 
        CancellationToken ct)
    {
        var cart = await context.Carts
            .AsNoTracking()
            .Where(x => 
                x.UserId == query.UserId)
            .FirstOrDefaultAsync(ct);

        if (cart is null)
            return Result<CartResponse>.Failure(
                CartErrors.NotFound);
        
        var cartItems = await context.CartItems
            .AsNoTracking()
            .Where(x => x.CartId == cart.Id)
            .ToListAsync(ct);

        var productIds = cartItems
            .Select(x => x.ProductId)
            .Distinct()
            .ToList();

        var products = await bus.InvokeAsync<List<ProductForCartResponse>>(
            new GetProductsForCartQuery(
                productIds), ct);

        var productMap = products.ToDictionary(x => x.Id);

        var result = cartItems
            .Select(item =>
            {
                if (!productMap.TryGetValue(
                        item.ProductId,
                        out var product))
                    return null;
                
                var totalPrice = product.Price * item.Quantity;

                return new CartItemResponse(
                    item.Id,
                    item.CartId,
                    product.ProductName,
                    product.ThumbnailUrl,
                    item.ProductId,
                    product.Price,
                    totalPrice,
                    item.Quantity);
            })
            .Where(x => x is not null)
            .Select(x => x!)
            .ToList();
        
        var total = result.Sum(x => 
            x.TotalPrice);
        
        return Result<CartResponse>.Success(
            new CartResponse(
                cart.Id, 
                cart.UserId,
                result,
                total));
    }
}