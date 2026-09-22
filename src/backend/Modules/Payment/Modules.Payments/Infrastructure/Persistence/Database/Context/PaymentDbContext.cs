using Microsoft.EntityFrameworkCore;
using Modules.Payments.DI;
using Modules.Payments.Domain;

namespace Modules.Payments.Infrastructure.Persistence.Database.Context;

public sealed class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        : base(options) {}
    
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(PaymentModuleMarker).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}