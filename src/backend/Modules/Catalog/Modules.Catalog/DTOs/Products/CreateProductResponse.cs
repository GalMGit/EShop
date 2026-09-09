namespace Modules.Catalog.DTOs.Products;

public sealed record CreateProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    Guid CategoryId,
    Guid BrandId,
    DateTime CreatedAt,
    Dictionary<string, object?> Specifications);