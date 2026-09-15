namespace EShop.Contracts.CQ.Catalog.Responses;

public sealed record ProductForOrderResponse(
    Guid ProductId,
    decimal Price,
    bool IsActive);