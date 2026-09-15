using EShop.Shared.Endpoint;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Orders.Infrastructure.Persistence.Database.Context;
using Wolverine;

namespace Modules.Orders.DI;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddOrderModule(
            IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(o =>
            {
                o.UseNpgsql(
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
        public void AddOrderMessaging()
        {
            options.Discovery.IncludeAssembly(
                typeof(OrderModuleMarker).Assembly);
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