namespace EShop.Contracts.CQ.Inventory.Responses;

public sealed record ProductForCartResponse(
    Guid Id, 
    decimal Price,
    bool IsActive);