namespace EShop.Contracts.CQ.Inventory.Commands;

public sealed record ReleaseInventoryCommand(
    Guid OrderId,
    IReadOnlyCollection<ReleaseInventoryItem> Items);

public sealed record ReleaseInventoryItem(
    Guid ProductId,
    int Quantity);