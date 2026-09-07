using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.DI;
using Modules.Identity.Domain;

namespace Modules.Identity.Infrastructure.Persistence.Database.Context;

public sealed class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options) {}

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Permission> Permissions => Set<Permission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IdentityModuleMarker).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}