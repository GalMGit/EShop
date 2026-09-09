using Microsoft.EntityFrameworkCore;
using Modules.Inventory.DI;

namespace Modules.Inventory.Infrastructure.Persistence.Database.Context;

public sealed class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(InventoryModuleMarker).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}