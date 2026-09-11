using EShop.Shared.Endpoint;
using EShop.Shared.ResultType;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Catalog.DTOs.Products;
using Wolverine;

namespace Modules.Catalog.Features.Products.SearchProducts;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products/search", async (
                string search,
                int page,
                int pageSize,
                IMessageBus bus,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<
                    Result<SearchProductsResponse>>(
                    new SearchProductsQuery(
                        search,
                        page,
                        pageSize),
                    ct);

                return result.ToHttpResponse();
            })
            .AllowAnonymous()
            .WithTags(Tags.Catalog);
    }
}