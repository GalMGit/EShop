namespace Modules.Cart.Features.RemoveFromCart;

public sealed record RemoveFromCartCommand(
    Guid ProductId, 
    Guid UserId);