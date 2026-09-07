using System.Reflection;
using EShop.Shared.Endpoint;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Identity.DI;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddIdentityModule(
            IConfiguration configuration)
        {
            services.AddEndpoints(Assembly.GetExecutingAssembly());
            
            return services;
        }
    }
}

public sealed class IdentityModuleMarker;
