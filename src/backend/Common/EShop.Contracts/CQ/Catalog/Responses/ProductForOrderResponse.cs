namespace EShop.Contracts.CQ.Catalog.Responses;

public sealed record ProductForOrderResponse(
    Guid ProductId,
    string Name,
    decimal Price,
    bool IsActive);