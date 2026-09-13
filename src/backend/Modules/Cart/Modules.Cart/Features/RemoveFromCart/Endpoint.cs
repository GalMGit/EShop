using System.Security.Claims;
using EShop.Shared.Endpoint;
using EShop.Shared.ResultType;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wolverine;

namespace Modules.Cart.Features.RemoveFromCart;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("cart/items/{productId:guid}", async (
                Guid productId,
                ClaimsPrincipal user,
                IMessageBus bus,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<Result>(
                    new RemoveFromCartCommand(
                        productId, 
                        user.GetUserId()), ct);

                return result.ToHttpResponse();
            })
            .RequireAuthorization()
            .WithTags(Tags.Cart);

    }
}