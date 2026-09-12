using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Cart.Infrastructure.Persistence.Database.Context;
using Wolverine;

namespace Modules.Cart.DI;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCartModule(
            IConfiguration configuration)
        {
            services.AddDbContext<CartDbContext>(o =>
            {
                o.UseNpgsql(
                    configuration.GetConnectionString(
                        "CartDatabase"));
            });

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
        public void AddCartMessaging()
        {
            options.Discovery.IncludeAssembly(
                typeof(CartModuleMarker).Assembly);
        }
    }
    
}

public sealed record CartModuleMarker;