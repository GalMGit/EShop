using EShop.Shared.Endpoint;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Inventory.Infrastructure.Persistence.Database.Context;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Persistence.Durability;
using Wolverine.Postgresql;

namespace Modules.Inventory.DI;

public static class DependencyInjection
{
    extension(WolverineOptions options)
    {
        public void AddInventoryMessaging(
            IConfiguration configuration)
        {
            options.Discovery.IncludeAssembly(
                typeof(InventoryModuleMarker).Assembly);
            
            options.PersistMessagesWithPostgresql(
                    configuration.GetConnectionString(
                        "InventoryDatabase")!,
                    role: MessageStoreRole.Ancillary)
                .Enroll<InventoryDbContext>();
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
            services.AddDbContextWithWolverineIntegration<InventoryDbContext>(
                options =>
                {
                    options.UseNpgsql(
                        configuration.GetConnectionString(
                            "InventoryDatabase"));
                });
            
            services.AddEndpoints(
                typeof(InventoryModuleMarker).Assembly);

            return services;
        }
    }
}

public sealed record InventoryModuleMarker;