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
                LinkGenerator linkGenerator,
                HttpContext httpContext,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<
                    Result<CreateProductResponse>>(
                        new CreateProductCommand(
                            request), ct);
                
                if (result.IsFailure)
                    return result.ToHttpResponse();

                var location = linkGenerator.GetUriByName(
                    httpContext,
                    EndpointNames.GetProduct,
                    new { id = result.Value!.Id });

                return Results.Created(
                    location,
                    result.Value);
            })
            .RequireAuthorization(PermissionNames.ProductsWrite)
            .WithTags(Tags.Catalog)
            .AddEndpointFilter<FluentValidationFilter<CreateProductRequest>>();
    }
}