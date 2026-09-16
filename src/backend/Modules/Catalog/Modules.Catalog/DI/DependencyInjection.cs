using Elastic.Clients.Elasticsearch;
using EShop.Contracts.Identity.Events;
using EShop.Shared.Endpoint;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;
using Modules.Catalog.Infrastructure.Search;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Persistence.Durability;
using Wolverine.Postgresql;

namespace Modules.Catalog.DI;

public static class DependencyInjection
{
    extension(WolverineOptions options)
    {
        public void AddCatalogMessaging(
            IConfiguration configuration)
        {
            options.Discovery.IncludeAssembly(
                typeof(CatalogModuleMarker).Assembly);

            options.PersistMessagesWithPostgresql(
                    configuration.GetConnectionString(
                        "CatalogDatabase")!,
                    role: MessageStoreRole.Ancillary)
                .Enroll<CatalogDbContext>();
        }
    }
    
    extension(IServiceProvider services)
    {
        public async Task InitializeCatalogAsync()
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<CatalogDbContext>();

            await db.Database.MigrateAsync();
        }
    }
    
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCatalogModule(
            IConfiguration configuration)
        {
            services.AddDbContextWithWolverineIntegration<CatalogDbContext>(
                options =>
                {
                    options.UseNpgsql(
                        configuration.GetConnectionString(
                            "CatalogDatabase"),
                        npgsqlOptions =>
                        {
                            npgsqlOptions.ConfigureDataSource(
                                dataSourceBuilder =>
                                {
                                    dataSourceBuilder.EnableDynamicJson();
                                });
                        });
                });
            
            services.Configure<ElasticsearchOptions>(
                configuration.GetSection("Elasticsearch"));

            services.AddSingleton(sp =>
            {
                var options = sp
                    .GetRequiredService<IOptions<ElasticsearchOptions>>()
                    .Value;

                var settings = new ElasticsearchClientSettings(
                    new Uri(options.Url));

                return new ElasticsearchClient(settings);
            });
            
            services.AddEndpoints(
                typeof(CatalogModuleMarker).Assembly);
            
            services.AddValidatorsFromAssembly(
                typeof(CatalogModuleMarker).Assembly);

            return services;
        }
    }
}

public sealed record CatalogModuleMarker;