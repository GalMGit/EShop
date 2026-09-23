namespace Modules.Catalog.DTOs.Products;

public sealed record ProductListItemResponse(
    Guid Id,
    string Name,
    string? ThumbnailUrl,
    decimal Price,
    Guid CategoryId,
    Guid BrandId,
    DateTime CreatedAt,
    int AvailableQuantity);