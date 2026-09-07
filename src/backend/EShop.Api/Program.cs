
using EShop.Api.DI;
using Modules.Identity.DI;
using EShop.Shared.Endpoint;
using JasperFx.CodeGeneration.Model;
using Scalar.AspNetCore;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine(opt =>
{
    opt.ServiceLocationPolicy = ServiceLocationPolicy.AlwaysAllowed;
    opt.Discovery.IncludeAssembly(
        typeof(IdentityModuleMarker).Assembly);
});

builder.Services.AddConfiguration(builder.Configuration);

var app = builder.Build();

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

app.Run();