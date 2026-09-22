namespace EShop.Contracts.CQ.Inventory.Commands;

public sealed record CommitInventoryCommand(
    Guid OrderId,
    IReadOnlyCollection<CommitInventoryItem> Items);


public sealed record CommitInventoryItem(
    Guid ProductId,
    int Quantity);