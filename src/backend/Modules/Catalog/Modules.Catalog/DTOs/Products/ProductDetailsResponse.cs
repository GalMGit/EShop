namespace Modules.Catalog.DTOs.Products;

public sealed record ProductDetailsResponse(
    Guid Id,
    string Name,
    string Description,
    string? MediaUrl,
    decimal Price,
    Guid CategoryId,
    Guid BrandId,
    DateTime CreatedAt,
    Dictionary<string, object?> Specifications,
    int AvailableQuantity);