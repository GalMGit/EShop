using EShop.Shared.Endpoint;
using EShop.Shared.Names;
using EShop.Shared.ResultType;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wolverine;

namespace Modules.Identity.Features.ChangeRole;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("users/role/change", async (
                ChangeRoleRequest request,
                IMessageBus bus,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<Result>(
                    new ChangeRoleCommand(
                        request), ct);

                return result.ToHttpResponse();
            })
            .WithTags(Tags.Identity)
            .RequireAuthorization(PermissionNames.UsersManage);
    }
}