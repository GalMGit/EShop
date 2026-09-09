using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.DTOs.Products;
using Modules.Catalog.Errors;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;
using Modules.Catalog.Mapping.Products;

namespace Modules.Catalog.Features.Products.GetProduct;

public sealed class GetProductHandler(
    CatalogDbContext context)
{
    public async Task<Result<ProductResponse>> Handle(
        GetProductQuery query,
        CancellationToken ct)
    {
        var product = await context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.Id, ct);

        if (product is null)
            return Result<ProductResponse>.Failure(
                ProductErrors.NotFound);

        return Result<ProductResponse>.Success(
            product.ToDto());
    }
}