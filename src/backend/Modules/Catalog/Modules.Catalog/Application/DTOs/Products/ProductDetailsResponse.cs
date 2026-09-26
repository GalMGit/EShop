namespace Modules.Catalog.Application.DTOs.Products;

public sealed record ProductDetailsResponse(
    Guid Id,
    string Name,
    string Brand,
    string Description,
    string? MediaUrl,
    decimal Price,
    Guid CategoryId,
    Guid BrandId,
    DateTime CreatedAt,
    Dictionary<string, object?> Specifications,
    int AvailableQuantity);