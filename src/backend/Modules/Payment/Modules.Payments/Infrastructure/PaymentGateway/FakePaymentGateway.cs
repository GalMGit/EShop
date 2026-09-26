using Modules.Payments.Application.Abstractions;
using Modules.Payments.Application.DTOs;

namespace Modules.Payments.Infrastructure.PaymentGateway;

public sealed class FakePaymentGateway : IPaymentGateway
{
    public Task<PaymentResult> ChargeAsync(
        PaymentRequest request,
        CancellationToken ct)
        => Task.FromResult(
            new PaymentResult(
                IsSuccessful: true,
                ProviderPaymentId: $"fake_{Guid.CreateVersion7()}",
                Error: null));
}