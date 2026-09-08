using EShop.Shared.Endpoint;
using EShop.Shared.ResultType;
using EShop.Shared.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wolverine;

namespace Modules.Identity.Features.CreateUser;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/register", async (
                CreateUserRequest request,
                IMessageBus command,
                CancellationToken ct) =>
            {
                var result = await command.InvokeAsync<Result>(
                    new CreateUserCommand(
                        request), ct);

                return result.ToHttpResponse();
            })
            .AllowAnonymous()
            .WithTags(Tags.Identity)
            .AddEndpointFilter<FluentValidationFilter<CreateUserRequest>>();
    }
}