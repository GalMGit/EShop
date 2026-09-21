namespace EShop.Contracts.Events.Inventory;

public sealed record InventoryReservedEvent(
    Guid OrderId);