using EShop.Shared.Endpoint;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Orders.Infrastructure.Persistence.Database.Context;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Persistence.Durability;
using Wolverine.Postgresql;

namespace Modules.Orders.DI;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddOrderModule(
            IConfiguration configuration)
        {
            services.AddDbContextWithWolverineIntegration<
                OrderDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString(
                        "OrderDatabase"));
            });
            
            services.AddEndpoints(
                typeof(OrderModuleMarker).Assembly);

            return services;
        }
    }
    
    extension(WolverineOptions options)
    {
        public void AddOrderMessaging(
            IConfiguration configuration)
        {
            options.Discovery.IncludeAssembly(
                typeof(OrderModuleMarker).Assembly);

            options.PersistMessagesWithPostgresql(
                    configuration.GetConnectionString(
                        "OrderDatabase")!,
                    role: MessageStoreRole.Ancillary)
                .Enroll<OrderDbContext>();
        }
    }
    
    extension(IServiceProvider services)
    {
        public async Task InitializeOrderAsync()
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<OrderDbContext>();

            await db.Database.MigrateAsync();
        }
    }
}


public sealed record OrderModuleMarker;