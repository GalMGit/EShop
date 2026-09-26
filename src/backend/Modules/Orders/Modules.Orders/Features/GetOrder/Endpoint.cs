using System.Security.Claims;
using EShop.Shared.Endpoint;
using EShop.Shared.Names;
using EShop.Shared.ResultType;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Orders.Application.DTOs;
using Wolverine;

namespace Modules.Orders.Features.GetOrder;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders/{orderId:guid}", async (
                Guid orderId,
                IMessageBus bus,
                ClaimsPrincipal user,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<
                    Result<OrderWithItemsResponse>>(
                        new GetOrderQuery(
                            orderId,
                            user.GetUserId()), ct);

                return result.ToHttpResponse();
            })
            .WithTags(Tags.Order)
            .RequireAuthorization(PermissionNames.OrdersRead);
    }
}