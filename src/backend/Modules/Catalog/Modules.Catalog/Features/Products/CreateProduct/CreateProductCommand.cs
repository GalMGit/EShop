using Modules.Catalog.Application.Abstractions.IServices.IMediaServices;

namespace Modules.Catalog.Features.Products.CreateProduct;

public sealed record CreateProductCommand(
    CreateProductRequest Request,
    UploadFile? Media,
    Dictionary<string, object?> Specifications);