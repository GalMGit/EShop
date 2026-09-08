using EShop.Shared.Endpoint;
using EShop.Shared.ResultType;
using EShop.Shared.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wolverine;

namespace Modules.Identity.Features.ConfirmEmail;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/email/confirm", async (
                ConfirmEmailRequest request,
                IMessageBus bus,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<Result>(
                    new ConfirmEmailCommand(
                        request), ct);

                return result.ToHttpResponse();
            })
            .WithTags(Tags.Identity)
            .AllowAnonymous()
            .AddEndpointFilter<FluentValidationFilter<ConfirmEmailRequest>>();
    }
}