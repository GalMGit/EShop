using EShop.Api.DI;
using Modules.Identity.DI;
using EShop.Shared.Endpoint;
using JasperFx.CodeGeneration.Model;
using Modules.Cart.DI;
using Modules.Catalog.DI;
using Modules.Inventory.DI;
using Scalar.AspNetCore;
using Serilog;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Host.UseWolverine(opt =>
{
    opt.UseRabbitMq(builder.Configuration.GetConnectionString("RabbitMq")!);
    opt.ServiceLocationPolicy = ServiceLocationPolicy.AlwaysAllowed;
    opt.AddIdentityMessaging();
    opt.AddCatalogMessaging();
    opt.AddInventoryMessaging();
    opt.AddCartMessaging();
});

builder.Services.AddConfiguration(builder.Configuration);

var app = builder.Build();

await app.Services.InitializeIdentityAsync();
await app.Services.InitializeCatalogAsync();
await app.Services.InitializeInventoryAsync();
await app.Services.InitializeCartAsync();

var api = app.MapGroup("/api/v1");
app.MapEndpoints(api);

app.MapOpenApi();
app.MapScalarApiReference("/docs",options =>
{
    options.WithTitle("EShop API")
        .AddPreferredSecuritySchemes("Bearer")
        .AddHttpAuthentication("Bearer", auth =>
        {
            auth.Token = string.Empty;
            auth.Description = "Bearer Token";
        });
});

app.UseAuthentication();
app.UseAuthorization();

app.Run();