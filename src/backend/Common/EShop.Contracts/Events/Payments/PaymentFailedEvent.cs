namespace EShop.Contracts.Events.Payments;

public sealed record PaymentFailedEvent(
    Guid PaymentId,
    Guid OrderId,
    string Reason);