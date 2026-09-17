using EShop.Api.DI;
using Modules.Identity.DI;
using EShop.Shared.Endpoint;
using JasperFx.CodeGeneration.Model;
using Modules.Cart.DI;
using Modules.Catalog.DI;
using Modules.Inventory.DI;
using Modules.Orders.DI;
using Scalar.AspNetCore;
using Serilog;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Persistence.Durability;
using Wolverine.Postgresql;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Host.UseWolverine(opt =>
{
    opt.UseRabbitMq(
        builder.Configuration.GetConnectionString(
            "RabbitMq")!);
    
    opt.UseEntityFrameworkCoreTransactions();
    
    opt.PersistMessagesWithPostgresql(
        builder.Configuration.GetConnectionString(
            "WolverineDatabase")!,
        role: MessageStoreRole.Main);
    
    opt.ServiceLocationPolicy = ServiceLocationPolicy.AlwaysAllowed;
    opt.AddIdentityMessaging(builder.Configuration);
    opt.AddCatalogMessaging(builder.Configuration);
    opt.AddInventoryMessaging(builder.Configuration);
    opt.AddCartMessaging(builder.Configuration);
    opt.AddOrderMessaging(builder.Configuration);
});

builder.Services.AddConfiguration(builder.Configuration);

var app = builder.Build();

await app.Services.InitializeIdentityAsync();
await app.Services.InitializeCatalogAsync();
await app.Services.InitializeInventoryAsync();
await app.Services.InitializeCartAsync();
await app.Services.InitializeOrderAsync();

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