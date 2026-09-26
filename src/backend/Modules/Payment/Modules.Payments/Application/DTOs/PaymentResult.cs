namespace Modules.Payments.Application.DTOs;

public sealed record PaymentResult(
    bool IsSuccessful, 
    string? ProviderPaymentId,
    string? Error);