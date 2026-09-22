using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Payments.Domain;

namespace Modules.Payments.Infrastructure.Persistence.Database.Configs;

public sealed class PaymentConfig
    : IEntityTypeConfiguration<Payment>
{
    public void Configure(
        EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(x => x.Method)
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(x => x.ProviderPaymentId)
            .HasMaxLength(200);

        builder.Property(x => x.FailureReason)
            .HasMaxLength(1000);

        builder.HasIndex(x => x.OrderId)
            .IsUnique();
    }
}