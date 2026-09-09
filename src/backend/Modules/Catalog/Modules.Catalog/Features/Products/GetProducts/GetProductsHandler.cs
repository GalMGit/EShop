using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.DTOs.Products;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;
using Modules.Catalog.Mapping.Products;

namespace Modules.Catalog.Features.Products.GetProducts;

public sealed class GetProductsHandler(
    CatalogDbContext context)
{
    public async Task<Result<List<ProductResponse>>> Handle(
        GetProductsQuery query,
        CancellationToken ct)
    {
        var products = await context.Products
            .AsNoTracking()
            .ToListAsync(ct);

        return Result<List<ProductResponse>>.Success(
            products
                .Select(x => x.ToDto())
                .ToList());
    }
}