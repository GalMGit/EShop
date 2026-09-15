using Microsoft.EntityFrameworkCore;
using Modules.Orders.DI;
using Modules.Orders.Domain;

namespace Modules.Orders.Infrastructure.Persistence.Database.Context;

public sealed class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options)
        : base(options) {}

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(OrderModuleMarker).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}