using Microsoft.EntityFrameworkCore;
using Modules.Cart.DI;
using Modules.Cart.Domain;

namespace Modules.Cart.Infrastructure.Persistence.Database.Context;

public sealed class CartDbContext : DbContext
{
    public CartDbContext(DbContextOptions<CartDbContext> options) 
        : base(options) {}

    public DbSet<ShopCart> Carts => Set<ShopCart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CartModuleMarker).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}