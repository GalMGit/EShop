using EShop.Shared.Endpoint;
using EShop.Shared.Names;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Wolverine;

namespace Modules.Identity.Features.Test;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users", () => "Эндпоинт только для админа")
            .RequireAuthorization(PermissionNames.UsersRead);
    }
}