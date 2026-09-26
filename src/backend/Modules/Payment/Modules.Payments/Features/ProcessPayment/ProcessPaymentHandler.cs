using EShop.Contracts.CQ.Payments;
using EShop.Contracts.Events.Payments;
using Microsoft.EntityFrameworkCore;
using Modules.Payments.Application.Abstractions;
using Modules.Payments.Application.DTOs;
using Modules.Payments.Domain;
using Modules.Payments.Infrastructure.Persistence.Database.Context;
using Wolverine;
using Wolverine.Attributes;

namespace Modules.Payments.Features.ProcessPayment;

[Transactional(typeof(PaymentDbContext))]
public sealed class ProcessPaymentHandler(
    PaymentDbContext context,
    IPaymentGateway gateway,
    IMessageBus bus)
{
    public async Task Handle(
        ProcessPaymentCommand command,
        CancellationToken ct)
    {
        var existingPayment = await context.Payments
            .SingleOrDefaultAsync(x => 
                x.OrderId == command.OrderId, ct);

        if (existingPayment is not null)
            return;

        var payment = new Payment
        {
            Id = Guid.CreateVersion7(),
            OrderId = command.OrderId,
            UserId = command.UserId,
            Amount = command.Amount,
            Currency = command.Currency,
            Status = PaymentStatus.Processing,
            Method = PaymentMethod.Card,
            CreatedAt = DateTime.UtcNow
        };

        await context.Payments.AddAsync(payment, ct);

        var result = await gateway.ChargeAsync(
            new PaymentRequest(
                payment.Id,
                payment.OrderId,
                payment.Amount,
                payment.Currency), ct);

        if (!result.IsSuccessful)
        {
            payment.Status = PaymentStatus.Failed;
            payment.FailureReason = result.Error;

            await bus.PublishAsync(new PaymentFailedEvent(
                payment.Id,
                payment.OrderId,
                result.Error ?? "Payment failed"));

            return;
        }

        payment.Status = PaymentStatus.Succeeded;
        payment.ProviderPaymentId = result.ProviderPaymentId;
        payment.CompletedAt = DateTime.UtcNow;
        
        await bus.PublishAsync(
            new PaymentSucceededEvent(
                payment.Id,
                payment.OrderId,
                payment.Amount,
                payment.Currency));
    }
}