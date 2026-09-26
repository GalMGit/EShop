using System.Text.Json;
using EShop.Shared.Endpoint;
using EShop.Shared.Names;
using EShop.Shared.ResultType;
using EShop.Shared.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Modules.Catalog.Application.Abstractions.IServices.IMediaServices;
using Modules.Catalog.Application.DTOs.Products;
using Wolverine;

namespace Modules.Catalog.Features.Products.CreateProduct;

public sealed class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products", async (
                [FromForm] CreateProductRequest request,
                IMessageBus bus,
                LinkGenerator linkGenerator,
                HttpContext httpContext,
                CancellationToken ct) =>
            {
                var upload = request.Media is null
                    ? null
                    : new UploadFile
                    {
                        FileName = request.Media.FileName,
                        ContentType = request.Media.ContentType,
                        Length = request.Media.Length,
                        Stream = request.Media.OpenReadStream()
                    };

                await using (upload)
                {
                    var specifications =
                        JsonSerializer.Deserialize<Dictionary<string, object?>>(
                            request.Specifications)
                        ?? new Dictionary<string, object?>();
                    
                    var command = new CreateProductCommand(
                        request,
                        upload,
                        specifications);
                    
                    var result = await bus.InvokeAsync<
                        Result<CreateProductResponse>>(
                            command, ct);
                
                    if (result.IsFailure)
                        return result.ToHttpResponse();
                    
                    var location = linkGenerator.GetUriByName(
                        httpContext,
                        EndpointNames.GetProduct,
                        new { id = result.Value!.Id });

                    return Results.Created(
                        location,
                        result.Value);
                }
            })
            .RequireAuthorization(PermissionNames.ProductsWrite)
            .WithTags(Tags.Catalog)
            .DisableAntiforgery()
            .AddEndpointFilter<FluentValidationFilter<CreateProductRequest>>();
    }
}