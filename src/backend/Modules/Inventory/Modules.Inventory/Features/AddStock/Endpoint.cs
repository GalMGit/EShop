using EShop.Shared.Endpoint;
using EShop.Shared.Names;
using EShop.Shared.ResultType;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wolverine;

namespace Modules.Inventory.Features.AddStock;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("stocks", async (
                AddStockRequest request,
                IMessageBus bus,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<Result>(
                    new AddStockCommand(
                        request), ct);

                return result.ToHttpResponse();
            })
            .WithTags(Tags.Inventory)
            .RequireAuthorization(PermissionNames.StockManage);
    }
}