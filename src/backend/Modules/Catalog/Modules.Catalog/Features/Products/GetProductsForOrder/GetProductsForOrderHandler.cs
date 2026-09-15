using EShop.Contracts.CQ.Catalog;
using EShop.Contracts.CQ.Catalog.Responses;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;

namespace Modules.Catalog.Features.Products.GetProductsForOrder;

public sealed class GetProductsForOrderHandler(
    CatalogDbContext context)
{
    public async Task<
        Result<IReadOnlyCollection<ProductForOrderResponse>>> Handle(
        GetProductsForOrderQuery query,
        CancellationToken ct)
    {
        var products = await context.Products
            .AsNoTracking()
            .Where(x => query.ProductIds
                .Contains(x.Id))
            .Select(x => new ProductForOrderResponse(
                x.Id,
                x.Price,
                x.IsActive))
            .ToListAsync(ct);

        return Result<
            IReadOnlyCollection<ProductForOrderResponse>>.Success(
            products);
    }
}