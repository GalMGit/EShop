namespace Modules.Orders.Application.DTOs;

public sealed record OrderResponse(
    Guid Id,
    decimal TotalAmount,
    string Status,
    DateTime CreatedAt);