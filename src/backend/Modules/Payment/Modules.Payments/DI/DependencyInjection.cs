using EShop.Shared.Endpoint;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Payments.Application.Abstractions;
using Modules.Payments.Infrastructure.PaymentGateway;
using Modules.Payments.Infrastructure.Persistence.Database.Context;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Persistence.Durability;
using Wolverine.Postgresql;

namespace Modules.Payments.DI;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPaymentModule(
            IConfiguration configuration)
        {
            services.AddDbContextWithWolverineIntegration<
                PaymentDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString(
                        "PaymentDatabase"));
            });
            
            services.AddEndpoints(
                typeof(PaymentModuleMarker).Assembly);

            services.AddScoped<IPaymentGateway, FakePaymentGateway>();

            return services;
        }
    }
    
    extension(WolverineOptions options)
    {
        public void AddPaymentMessaging(
            IConfiguration configuration)
        {
            options.Discovery.IncludeAssembly(
                typeof(PaymentModuleMarker).Assembly);

            options.PersistMessagesWithPostgresql(
                    configuration.GetConnectionString(
                        "PaymentDatabase")!,
                    role: MessageStoreRole.Ancillary)
                .Enroll<PaymentDbContext>();
        }
    }
    
    extension(IServiceProvider services)
    {
        public async Task InitializePaymentAsync()
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<PaymentDbContext>();

            await db.Database.MigrateAsync();
        }
    }
}

public sealed record PaymentModuleMarker;