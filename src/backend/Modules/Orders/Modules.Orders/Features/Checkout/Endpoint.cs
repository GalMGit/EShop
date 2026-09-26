using System.Security.Claims;
using EShop.Shared.Endpoint;
using EShop.Shared.ResultType;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Orders.Application.DTOs;
using Wolverine;

namespace Modules.Orders.Features.Checkout;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("orders/checkout", async (
                IMessageBus bus,
                ClaimsPrincipal user,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<
                    Result<CheckoutResponse>>(
                        new CheckoutCommand(
                            user.GetUserId()), ct);

                return result.ToHttpResponse();
            })
            .WithTags(Tags.Order)
            .RequireAuthorization();
    }
}