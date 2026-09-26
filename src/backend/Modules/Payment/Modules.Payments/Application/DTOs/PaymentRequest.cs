namespace Modules.Payments.Application.DTOs;

public sealed record PaymentRequest(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount,
    string Currency);