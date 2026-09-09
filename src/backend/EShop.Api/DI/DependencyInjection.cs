
using Microsoft.OpenApi;
using Modules.Catalog.DI;
using Modules.Identity.DI;

namespace EShop.Api.DI;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public void AddConfiguration(
            IConfiguration configuration)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    var bearerScheme = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header
                    };

                    document.Components ??= new OpenApiComponents();
                    document.AddComponent("Bearer", bearerScheme);

                    var securityRequirement = new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                    };

                    foreach (var operation in document.Paths.Values
                                 .SelectMany(path => path.Operations!))
                    {
                        operation.Value.Security ??= [];
                        operation.Value.Security.Add(securityRequirement);
                    }

                    return Task.CompletedTask;
                });
            });
            
            services.AddIdentityModule(configuration);
            services.AddCatalogModule(configuration);
        }
        
    }
}