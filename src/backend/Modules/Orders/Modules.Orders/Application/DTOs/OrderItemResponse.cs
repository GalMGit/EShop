namespace Modules.Orders.Application.DTOs;

public sealed record OrderItemResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string? ProductImageUrl,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice);