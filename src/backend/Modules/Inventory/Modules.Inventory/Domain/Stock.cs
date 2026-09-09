namespace Modules.Inventory.Domain;

public sealed class Stock
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public int ReservedQuantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public int AvailableQuantity =>
        Quantity - ReservedQuantity;
}