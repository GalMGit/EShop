using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Inventory.Infrastructure.Persistence.Database.Context;
using Wolverine;

namespace Modules.Inventory.DI;

public static class DependencyInjection
{
    extension(WolverineOptions options)
    {
        public void AddInventoryMessaging()
        {
            options.Discovery.IncludeAssembly(
                typeof(InventoryModuleMarker).Assembly);
        }
    }
    
    extension(IServiceProvider services)
    {
        public async Task InitializeInventoryAsync()
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<InventoryDbContext>();

            await db.Database.MigrateAsync();
        }
    }
    
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInventoryModule(
            IConfiguration configuration)
        {
            services.AddDbContext<InventoryDbContext>(o =>
            {
                o.UseNpgsql(configuration.GetConnectionString("InventoryDatabase"));
            });

            return services;
        }
    }
}

public sealed record InventoryModuleMarker;