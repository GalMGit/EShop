namespace Modules.Cart.Features.RemoveFromCart;

public sealed record RemoveFromCartCommand(
    Guid ItemId, 
    Guid UserId);