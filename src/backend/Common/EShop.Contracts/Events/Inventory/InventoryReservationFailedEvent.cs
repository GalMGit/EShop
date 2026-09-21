namespace EShop.Contracts.Events.Inventory;

public sealed record InventoryReservationFailedEvent(
    Guid OrderId,
    string Message);