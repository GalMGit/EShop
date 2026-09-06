using EShop.Shared.Endpoint;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Wolverine;

namespace EShop.Modules.Identity.Features.Test;

public sealed class TestEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("test", async (
                IMessageBus query) =>
            {
                var result = await query.InvokeAsync<TestResponse>(
                    new TestCommand());

                return result;
            })
            .AllowAnonymous();
    }
}