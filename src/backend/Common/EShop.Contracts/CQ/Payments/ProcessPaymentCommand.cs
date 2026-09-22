namespace EShop.Contracts.CQ.Payments;

public sealed record ProcessPaymentCommand(
    Guid OrderId,
    Guid UserId,
    decimal Amount,
    string Currency);