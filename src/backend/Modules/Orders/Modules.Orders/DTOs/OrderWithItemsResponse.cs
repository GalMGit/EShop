using Modules.Orders.Domain;

namespace Modules.Orders.DTOs;

public sealed record OrderWithItemsResponse(
    Guid Id,
    decimal TotalAmount,
    OrderStatus Status,
    DateTime CreatedAt,
    List<OrderItemResponse> Items);
