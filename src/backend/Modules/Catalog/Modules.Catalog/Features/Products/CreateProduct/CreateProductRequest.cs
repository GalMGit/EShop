using Microsoft.AspNetCore.Http;

namespace Modules.Catalog.Features.Products.CreateProduct;

public sealed record CreateProductRequest(
    string Name,
    string Description,
    decimal Price,
    IFormFile? Media,
    Guid CategoryId,
    Guid BrandId,
    string Specifications);
    