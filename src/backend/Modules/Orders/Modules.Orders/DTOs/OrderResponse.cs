using Modules.Orders.Domain;

namespace Modules.Orders.DTOs;

public sealed record OrderResponse(
    Guid Id,
    decimal TotalAmount,
    string Status,
    DateTime CreatedAt);