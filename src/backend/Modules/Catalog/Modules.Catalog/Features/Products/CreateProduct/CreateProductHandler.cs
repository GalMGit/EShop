using EShop.Contracts.Catalog.Products;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Application.Abstractions.IServices.IMediaServices;
using Modules.Catalog.Domain;
using Modules.Catalog.DTOs.Products;
using Modules.Catalog.Errors;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;
using Wolverine;
using Wolverine.Attributes;

namespace Modules.Catalog.Features.Products.CreateProduct;

[Transactional(typeof(CatalogDbContext))]
public sealed class CreateProductHandler(
    CatalogDbContext context,
    IFileStorageService storageService,
    IMessageBus bus)
{
    public async Task<Result<CreateProductResponse>> Handle(
        CreateProductCommand command,
        CancellationToken ct)
    {
        var categoryExists = await context.Categories
            .AnyAsync(x => 
                x.Id == command.Request.CategoryId, ct);

        if (!categoryExists)
            return Result<CreateProductResponse>.Failure(
                CategoryErrors.NotFound);

        var brandExists = await context.Brands
            .AnyAsync(x => 
                x.Id == command.Request.BrandId, ct);
        
        if(!brandExists)
            return Result<CreateProductResponse>.Failure(
                BrandErrors.NotFound);

        var product = new Product
        {
            Id = Guid.CreateVersion7(),
            Name = command.Request.Name,
            Price = command.Request.Price,
            Description = command.Request.Description,
            CategoryId = command.Request.CategoryId,
            BrandId = command.Request.BrandId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            Specifications = command.Specifications
        };
        
        var (mediaPath, thumbnailPath) = await UploadMediaAsync(
            command, 
            product.Id,
            ct);

        product.MediaPath = mediaPath;
        product.ThumbnailPath = thumbnailPath;

        await context.Products.AddAsync(product, ct);

        await bus.PublishAsync(
            new ProductCreatedEvent(
                product.Id));

        return Result<CreateProductResponse>.Success(
            new CreateProductResponse(product.Id));
    }

    private async Task<(
        string? MediaUrl, 
        string? ThumbnailUrl)> UploadMediaAsync(
        CreateProductCommand command,
        Guid productId,
        CancellationToken ct)
    {
        if (command.Media is null)
            return (null, null);

        var upload = await storageService.UploadPictureAsync(
            command.Media,
            productId, ct);

        return (
            upload.RelativePath,
            upload.ThumbnailRelativePath
            );
    }
}