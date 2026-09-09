namespace Modules.Catalog.Domain;

public sealed class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }

    public Dictionary<string, object> Specifications { get; set; } = [];

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}