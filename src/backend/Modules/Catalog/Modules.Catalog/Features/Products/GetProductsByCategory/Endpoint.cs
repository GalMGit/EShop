using EShop.Shared.Endpoint;
using EShop.Shared.ResultType;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Modules.Catalog.DTOs.Products;
using Wolverine;

namespace Modules.Catalog.Features.Products.GetProductsByCategory;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products/categories/{categoryId:guid}", async (
                Guid categoryId,
                IMessageBus bus,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<
                    Result<List<ProductListItemResponse>>>(
                    new GetProductsByCategoryQuery(
                        categoryId), ct);

                return result.ToHttpResponse();
            })
            .AllowAnonymous()
            .WithTags(Tags.Catalog);
    }
}