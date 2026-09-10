namespace EShop.Contracts.CQ.Inventory.Responses;

public sealed record StockResponse(
    Guid ProductId, 
    int AvailableQuantity);