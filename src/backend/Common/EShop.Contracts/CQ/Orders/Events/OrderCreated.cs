namespace EShop.Contracts.CQ.Orders.Events;

public sealed record OrderCreatedEvent(
    Guid OrderId,
    IReadOnlyCollection<OrderCreatedItem> Items);
    
public sealed record OrderCreatedItem(
    Guid ProductId,
    int Quantity);