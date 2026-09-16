using System.Security.Claims;
using EShop.Shared.Endpoint;
using EShop.Shared.Names;
using EShop.Shared.ResultType;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Orders.DTOs;
using Wolverine;

namespace Modules.Orders.Features.GetOrders;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders", async (
                ClaimsPrincipal user,
                IMessageBus bus,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<
                    Result<List<OrderResponse>>>(
                        new GetOrdersQuery(
                            user.GetUserId()), ct);

                return result.ToHttpResponse();
            })
            .WithTags(Tags.Order)
            .RequireAuthorization(PermissionNames.OrdersRead);
    }
}