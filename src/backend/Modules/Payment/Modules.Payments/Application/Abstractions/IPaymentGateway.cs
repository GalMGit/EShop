using Modules.Payments.Application.DTOs;

namespace Modules.Payments.Application.Abstractions;

public interface IPaymentGateway
{
    Task<PaymentResult> ChargeAsync(
        PaymentRequest request,
        CancellationToken ct);
}