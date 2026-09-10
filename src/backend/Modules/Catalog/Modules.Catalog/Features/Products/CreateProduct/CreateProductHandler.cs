using EShop.Contracts.Catalog.Products;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Domain;
using Modules.Catalog.DTOs.Products;
using Modules.Catalog.Errors;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;
using Modules.Catalog.Mapping.Products;
using Wolverine;

namespace Modules.Catalog.Features.Products.CreateProduct;

public sealed class CreateProductHandler(
    CatalogDbContext context,
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
            Specifications = command.Request.Specifications
        };

        await context.Products.AddAsync(product, ct);
        await context.SaveChangesAsync(ct);

        await bus.PublishAsync(
            new ProductCreatedEvent(
                product.Id));

        return Result<CreateProductResponse>.Success(
            new CreateProductResponse(product.Id));
    }
}