namespace Modules.Orders.Features.GetOrder;

public sealed record GetOrderQuery(
    Guid OrderId,
    Guid UserId);