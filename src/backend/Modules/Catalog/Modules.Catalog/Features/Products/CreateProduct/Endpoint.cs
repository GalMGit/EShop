using EShop.Shared.Endpoint;
using EShop.Shared.Names;
using EShop.Shared.ResultType;
using EShop.Shared.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Catalog.DTOs.Products;
using Wolverine;

namespace Modules.Catalog.Features.Products.CreateProduct;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products", async (
                CreateProductRequest request,
                IMessageBus bus,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<Result<ProductResponse>>(
                    new CreateProductCommand(
                        request), ct);

                return result.ToHttpResponse();
            })
            .RequireAuthorization(PermissionNames.ProductsWrite)
            .WithTags(Tags.Catalog)
            .AddEndpointFilter<FluentValidationFilter<CreateProductRequest>>();
    }
}