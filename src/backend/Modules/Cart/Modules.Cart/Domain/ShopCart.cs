namespace Modules.Cart.Domain;

public sealed class ShopCart
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<CartItem> Items { get; set; } = [];
}