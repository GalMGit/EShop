using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Domain;
using Modules.Catalog.DTOs.Brands;
using Modules.Catalog.Errors;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;
using Modules.Catalog.Mapping.Brands;

namespace Modules.Catalog.Features.Brands.CreateBrand;

public sealed class CreateBrandHandler(
    CatalogDbContext context)
{
    public async Task<Result<BrandResponse>> Handle(
        CreateBrandCommand command,
        CancellationToken ct)
    {
        var brandExists = await context.Brands
            .AnyAsync(x => 
                x.Name == command.Request.Name, ct);

        if (brandExists)
            return Result<BrandResponse>.Failure(
                BrandErrors.BrandExists);

        var brand = new Brand
        {
            Id = Guid.CreateVersion7(),
            Name = command.Request.Name,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await context.Brands.AddAsync(brand, ct);
        await context.SaveChangesAsync(ct);

        return Result<BrandResponse>.Success(
            brand.ToDto());
    }
}