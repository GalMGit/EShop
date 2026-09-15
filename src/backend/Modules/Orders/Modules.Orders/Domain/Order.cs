namespace Modules.Orders.Domain;

public sealed class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<OrderItem> Items { get; private set; } = [];
}