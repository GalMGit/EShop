using EShop.Api.DI;
using Modules.Identity.DI;
using EShop.Shared.Endpoint;
using JasperFx.CodeGeneration.Model;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.Infrastructure.Persistence.Database.Context;
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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<IdentityDbContext>();

    await db.Database.MigrateAsync();
}

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