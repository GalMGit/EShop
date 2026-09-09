namespace Modules.Catalog.Features.Products.CreateProduct;

public sealed record CreateProductResponse(
    string Name,
    string Description,
    decimal Price,
    Guid CategoryId,
    Guid BrandId,
    DateTime CreatedAt,
    Dictionary<string, object?> Specifications);