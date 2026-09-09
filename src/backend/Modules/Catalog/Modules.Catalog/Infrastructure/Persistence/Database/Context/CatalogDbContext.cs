using Microsoft.EntityFrameworkCore;
using Modules.Catalog.DI;
using Modules.Catalog.Domain;

namespace Modules.Catalog.Infrastructure.Persistence.Database.Context;

public sealed class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) 
        : base(options) {}
    
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Category> Categories => Set<Category>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CatalogModuleMarker).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}