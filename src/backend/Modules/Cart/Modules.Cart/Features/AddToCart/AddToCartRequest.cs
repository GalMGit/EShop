namespace Modules.Cart.Features.AddToCart;

public sealed record AddToCartRequest(
    Guid ProductId,
    int Quantity);