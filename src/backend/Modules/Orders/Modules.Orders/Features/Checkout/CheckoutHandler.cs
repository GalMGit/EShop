using EShop.Contracts.CQ.Cart;
using EShop.Contracts.CQ.Cart.Responses;
using EShop.Contracts.CQ.Catalog;
using EShop.Contracts.CQ.Catalog.Responses;
using EShop.Shared.ResultType;
using Modules.Orders.Domain;
using Modules.Orders.DTOs;
using Modules.Orders.Errors;
using Modules.Orders.Infrastructure.Persistence.Database.Context;
using Wolverine;

namespace Modules.Orders.Features.Checkout;

public sealed class CheckoutHandler(
    OrderDbContext context,
    IMessageBus bus)
{
    public async Task<Result<CheckoutResponse>> Handle(
        CheckoutCommand command, 
        CancellationToken ct)
    {
        var cartResult = await bus.InvokeAsync<
            Result<CartForCheckoutResponse>>(
                new GetCartForCheckoutQuery(
                    command.UserId), ct);

        if (cartResult.IsFailure)
            return Result<CheckoutResponse>.Failure(
                cartResult.Error!);

        var cart = cartResult.Value!;

        if (cart.Items.Count == 0)
            return Result<CheckoutResponse>.Failure(
                OrderErrors.CartEmpty);

        var productIds = cart.Items
            .Select(x => x.ProductId)
            .Distinct()
            .ToArray();

        var productsResult = await bus.InvokeAsync<
            Result<IReadOnlyCollection<ProductForOrderResponse>>>(
                new GetProductsForOrderQuery(productIds), ct);

        if (productsResult.IsFailure)
            return Result<CheckoutResponse>.Failure(
                productsResult.Error!);

        var products = productsResult.Value!;

        if (products.Count != productIds.Length)
        {
            return Result<CheckoutResponse>.Failure(
                OrderErrors.ProductUnavailable);
        }

        if (products.Any(x => !x.IsActive))
        {
            return Result<CheckoutResponse>.Failure(
                OrderErrors.ProductUnavailable);
        }
        
        var order = new Order
        {
            Id = Guid.CreateVersion7(),
            UserId = command.UserId,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        
        foreach (var cartItem in cart.Items)
        {
            var product = products.Single(
                x => x.ProductId == cartItem.ProductId);

            order.Items.Add(new OrderItem
            {
                Id = Guid.CreateVersion7(),
                OrderId = order.Id,
                ProductId = product.ProductId,
                Quantity = cartItem.Quantity,
                ProductName = product.Name,
                UnitPrice = product.Price
            });
        }
        
        order.TotalAmount = order.Items.Sum(
            x => x.TotalPrice);
        
        context.Orders.Add(order);

        await context.SaveChangesAsync(ct);
        
        return Result<CheckoutResponse>.Success(
            new CheckoutResponse(
                order.Id,
                order.TotalAmount));
    }
}