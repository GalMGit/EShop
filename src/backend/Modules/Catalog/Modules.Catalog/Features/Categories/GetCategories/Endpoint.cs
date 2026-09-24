using EShop.Shared.Endpoint;
using EShop.Shared.ResultType;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Catalog.DTOs.Categories;
using Wolverine;

namespace Modules.Catalog.Features.Categories.GetCategories;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (
                IMessageBus bus,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<
                    Result<List<CategoryResponse>>>(
                    new GetCategoriesQuery(), ct);

                return result.ToHttpResponse();
            })
            .AllowAnonymous()
            .WithTags(Tags.Catalog);
    }
}