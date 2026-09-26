namespace Modules.Cart.Application.DTOs;

public sealed record CartResponse(
    Guid Id,
    Guid UserId,
    IReadOnlyCollection<CartItemResponse> Items,
    decimal Total);