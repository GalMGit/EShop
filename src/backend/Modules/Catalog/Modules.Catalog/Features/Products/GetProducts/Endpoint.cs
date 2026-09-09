using EShop.Shared.Endpoint;
using EShop.Shared.ResultType;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Catalog.DTOs.Products;
using Wolverine;

namespace Modules.Catalog.Features.Products.GetProducts;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products", async (
                IMessageBus bus,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<Result<List<ProductResponse>>>(
                    new GetProductsQuery(), ct);

                return result.ToHttpResponse();
            })
            .AllowAnonymous()
            .WithTags(Tags.Catalog);
    }
}