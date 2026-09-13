using System.Security.Claims;
using EShop.Shared.Endpoint;
using EShop.Shared.ResultType;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wolverine;

namespace Modules.Cart.Features.AddToCart;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("cart", async (
                AddToCartRequest request,
                IMessageBus bus,
                ClaimsPrincipal user,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<Result>(
                    new AddToCartCommand(
                        request, 
                        user.GetUserId()), ct);

                return result.ToHttpResponse();
            })
            .RequireAuthorization()
            .WithTags(Tags.Cart);
    }
}