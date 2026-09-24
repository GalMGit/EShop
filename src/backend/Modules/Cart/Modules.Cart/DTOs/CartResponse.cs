namespace Modules.Cart.DTOs;

public sealed record CartResponse(
    Guid Id,
    Guid UserId,
    IReadOnlyCollection<CartItemResponse> Items,
    decimal Total);