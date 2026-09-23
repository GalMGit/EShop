using EShop.Api.DI;
using Modules.Identity.DI;
using EShop.Shared.Endpoint;
using JasperFx.CodeGeneration.Model;
using Modules.Cart.DI;
using Modules.Catalog.DI;
using Modules.Inventory.DI;
using Modules.Orders.DI;
using Modules.Payments.DI;
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
    opt.Policies.UseDurableOutboxOnAllSendingEndpoints();
    opt.Policies.UseDurableInboxOnAllListeners();
    
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
    opt.AddPaymentMessaging(builder.Configuration);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendClient", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddConfiguration(builder.Configuration);

var app = builder.Build();

await app.Services.InitializeIdentityAsync();
await app.Services.InitializeCatalogAsync();
await app.Services.InitializeInventoryAsync();
await app.Services.InitializeCartAsync();
await app.Services.InitializeOrderAsync();
await app.Services.InitializePaymentAsync();

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

app.UseCors("FrontendClient");

app.UseAuthentication();
app.UseAuthorization();

app.Run();