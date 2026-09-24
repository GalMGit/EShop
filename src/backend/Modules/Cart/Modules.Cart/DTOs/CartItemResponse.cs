namespace Modules.Cart.DTOs;

public sealed record CartItemResponse(
    Guid Id,
    Guid CartId,
    string ProductName,
    string? ThumbnailUrl,
    Guid ProductId,
    decimal UnitPrice,
    decimal TotalPrice,
    int Quantity);