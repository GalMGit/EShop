namespace Modules.Orders.Domain;

public enum OrderStatus
{
    Pending = 0,
    StockReserved = 1,
    PaymentPending = 2,
    Paid = 3,
    Confirmed = 4,
    Cancelled = 5
}