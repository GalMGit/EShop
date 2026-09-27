namespace Modules.Orders.Application.DTOs;

public sealed record OrderWithItemsResponse(
    Guid Id,
    decimal TotalAmount,
    string Status,
    DateTime CreatedAt,
    string? CancellationReason,
    List<OrderItemResponse> Items);
