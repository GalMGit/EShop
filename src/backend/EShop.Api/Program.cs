using EShop.Modules.Identity.DI;
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

var app = builder.Build();


app.MapEndpoints();

app.MapOpenApi();
app.MapScalarApiReference("/docs",options =>
{
    options.WithTitle("VOIDApi")
        .AddPreferredSecuritySchemes("Bearer")
        .AddHttpAuthentication("Bearer", auth =>
        {
            auth.Token = string.Empty;
            auth.Description = "Bearer Token";
        });
});

app.Run();