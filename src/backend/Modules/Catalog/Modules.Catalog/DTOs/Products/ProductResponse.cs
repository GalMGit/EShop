namespace Modules.Catalog.DTOs.Products;

public sealed record ProductResponse(
    string Name,
    string Description,
    decimal Price,
    Guid CategoryId,
    Guid BrandId,
    DateTime CreatedAt,
    Dictionary<string, object?> Specifications);