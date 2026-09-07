using EShop.Shared.Endpoint;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wolverine;

namespace Modules.Identity.Features.CreateUser;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (
                CreateUserRequest request,
                IMessageBus command,
                CancellationToken ct) =>
            {
                await command.InvokeAsync(
                    new CreateUserCommand(
                        request), ct);
            })
            .AllowAnonymous()
            .WithTags("Identity");
    }
}