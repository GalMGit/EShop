namespace Modules.Cart.Features.AddToCart;

public sealed record AddToCartCommand(
    AddToCartRequest Request,
    Guid UserId);