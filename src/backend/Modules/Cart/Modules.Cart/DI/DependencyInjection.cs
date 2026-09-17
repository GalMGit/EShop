using EShop.Shared.Endpoint;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Cart.Infrastructure.Persistence.Database.Context;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Persistence.Durability;
using Wolverine.Postgresql;

namespace Modules.Cart.DI;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCartModule(
            IConfiguration configuration)
        {
            services.AddDbContextWithWolverineIntegration<CartDbContext>(
                options =>
                {
                    options.UseNpgsql(
                        configuration.GetConnectionString(
                            "CartDatabase"));
                });
            
            services.AddEndpoints(
                typeof(CartModuleMarker).Assembly);

            return services;
        }
    }
    
    extension(IServiceProvider services)
    {
        public async Task InitializeCartAsync()
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<CartDbContext>();

            await db.Database.MigrateAsync();
        }
    }
    
    extension(WolverineOptions options)
    {
        public void AddCartMessaging(
            IConfiguration configuration)
        {
            options.Discovery.IncludeAssembly(
                typeof(CartModuleMarker).Assembly);
            
            options.PersistMessagesWithPostgresql(
                    configuration.GetConnectionString(
                        "CartDatabase")!,
                    role: MessageStoreRole.Ancillary)
                .Enroll<CartDbContext>();
        }
    }
    
}

public sealed record CartModuleMarker;