namespace Modules.Catalog.Infrastructure.Search;

public sealed class ProductSearchDocument
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Price { get; init; }
    public Guid CategoryId { get; init; }
    public Guid BrandId { get; init; }
    public bool IsActive { get; init; }
    public Dictionary<string, object?> Specifications { get; init; } = [];
}