namespace Modules.Inventory.Features.AddStock;

public sealed record AddStockRequest(
    Guid ProductId,
    int Quantity);