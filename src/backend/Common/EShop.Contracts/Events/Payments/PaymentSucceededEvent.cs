namespace EShop.Contracts.Events.Payments;

public sealed record PaymentSucceededEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount,
    string Currency);