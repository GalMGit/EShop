namespace EShop.Contracts.CQ.Catalog.Responses;

public sealed record ProductForOrderResponse(
    Guid ProductId,
    string Name,
    string? ImageUrl,
    decimal Price,
    bool IsActive);