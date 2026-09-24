namespace Modules.Catalog.DTOs.Products;

public sealed record ProductListItemResponse(
    Guid Id,
    string Name,
    string Brand,
    string? ThumbnailUrl,
    decimal Price,
    Guid CategoryId,
    Guid BrandId,
    DateTime CreatedAt,
    int AvailableQuantity);