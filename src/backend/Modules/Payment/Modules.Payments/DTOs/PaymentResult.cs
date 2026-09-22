namespace Modules.Payments.DTOs;

public sealed record PaymentResult(
    bool IsSuccessful, 
    string? ProviderPaymentId,
    string? Error);