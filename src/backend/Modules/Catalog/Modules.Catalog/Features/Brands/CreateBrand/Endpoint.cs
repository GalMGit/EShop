using EShop.Shared.Endpoint;
using EShop.Shared.Names;
using EShop.Shared.ResultType;
using EShop.Shared.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Catalog.DTOs.Brands;
using Wolverine;

namespace Modules.Catalog.Features.Brands.CreateBrand;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("brands", async (
                CreateBrandRequest request,
                IMessageBus bus,
                CancellationToken ct) =>
            {
                var result = await bus.InvokeAsync<Result<BrandResponse>>(
                    new CreateBrandCommand(
                        request), ct);

                return result.ToHttpResponse();
            })
            .RequireAuthorization(PermissionNames.BrandWrite)
            .WithTags(Tags.Catalog)
            .AddEndpointFilter<FluentValidationFilter<CreateBrandRequest>>();
    }
}