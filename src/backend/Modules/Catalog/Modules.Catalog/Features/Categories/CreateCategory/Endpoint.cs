using EShop.Shared.Endpoint;
using EShop.Shared.Names;
using EShop.Shared.ResultType;
using EShop.Shared.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Catalog.DTOs.Categories;
using Wolverine;

namespace Modules.Catalog.Features.Categories.CreateCategory;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("categories", async (
                CreateCategoryRequest request,
                IMessageBus bus,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<
                    Result<CategoryResponse>>(
                        new CreateCategoryCommand(
                            request), ct);

                return result.ToHttpResponse();
            })
            .RequireAuthorization(PermissionNames.CategoryWrite)
            .WithTags(Tags.Catalog)
            .AddEndpointFilter<FluentValidationFilter<CreateCategoryRequest>>();
    }
}