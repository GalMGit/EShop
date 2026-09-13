namespace Modules.Cart.Features.AddToCart;

public sealed record AddToCartRequest(
    Guid CartId,
    Guid ProductId,
    int Quantity);