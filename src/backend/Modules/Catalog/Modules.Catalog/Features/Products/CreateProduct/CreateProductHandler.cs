using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Domain;
using Modules.Catalog.DTOs.Products;
using Modules.Catalog.Errors;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;
using Modules.Catalog.Mapping.Products;

namespace Modules.Catalog.Features.Products.CreateProduct;

public sealed class CreateProductHandler(
    CatalogDbContext context)
{
    public async Task<Result<ProductResponse>> Handle(
        CreateProductCommand command,
        CancellationToken ct)
    {
        var categoryExists = await context.Categories
            .AnyAsync(x => x.Id == command.Request.CategoryId, ct);

        if (!categoryExists)
            return Result<ProductResponse>.Failure(
                CategoryErrors.NotFound);

        var brandExists = await context.Brands
            .AnyAsync(x => x.Id == command.Request.BrandId, ct);
        
        if(!brandExists)
            return Result<ProductResponse>.Failure(
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

        return Result<ProductResponse>.Success(
            product.ToDto());
    }
}