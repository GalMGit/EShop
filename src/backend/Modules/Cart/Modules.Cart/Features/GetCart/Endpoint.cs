using System.Security.Claims;
using EShop.Shared.Endpoint;
using EShop.Shared.Names;
using EShop.Shared.ResultType;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Cart.Application.DTOs;
using Wolverine;

namespace Modules.Cart.Features.GetCart;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("cart", async (
                ClaimsPrincipal user,
                IMessageBus bus,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<
                    Result<CartResponse>>(
                        new GetCartQuery(
                            user.GetUserId()), ct);

                return result.ToHttpResponse();
            })
            .WithTags(Tags.Cart)
            .RequireAuthorization();
    }
}