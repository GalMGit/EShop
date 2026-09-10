using Microsoft.EntityFrameworkCore;
using Modules.Inventory.DI;
using Modules.Inventory.Domain;

namespace Modules.Inventory.Infrastructure.Persistence.Database.Context;

public sealed class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options) {}

    public DbSet<Stock> Stocks => Set<Stock>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(InventoryModuleMarker).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}