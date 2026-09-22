namespace EShop.Contracts.Events.Inventory;

public sealed record InventoryCommittedEvent(
    Guid OrderId);