namespace EShop.Contracts.CQ.Catalog.Responses;

public sealed record ProductForCartResponse(
    Guid Id, 
    string? ThumbnailUrl,
    string ProductName,
    decimal Price,
    bool IsActive);