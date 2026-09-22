namespace Modules.Payments.Domain;

public sealed class Payment
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "RUB";
    public PaymentStatus Status { get; set; }
    public PaymentMethod Method { get; set; }
    public string? ProviderPaymentId { get; set; }
    public string? FailureReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}