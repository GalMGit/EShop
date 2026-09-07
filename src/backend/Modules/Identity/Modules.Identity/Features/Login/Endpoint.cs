using EShop.Shared.Endpoint;
using EShop.Shared.ResultType;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wolverine;

namespace Modules.Identity.Features.Login;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/login", async (
                LoginRequest request,
                IMessageBus command,
                CancellationToken ct) =>
            {
                var result = await command.InvokeAsync<Result<LoginResponse>>(
                    new LoginCommand(
                        request), ct);

                return result.ToHttpResponse();
            })
            .AllowAnonymous()
            .WithTags("Identity");
    }
}