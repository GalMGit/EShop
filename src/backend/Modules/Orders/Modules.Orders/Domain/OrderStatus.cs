namespace Modules.Orders.Domain;

public enum OrderStatus
{
    Pending = 0,
    ReservingInventory = 1,
    InventoryReserved = 2,
    ProcessingPayment = 3,
    Paid = 4,
    Completed = 5,
    Cancelling = 6,
    Cancelled = 7
}