using EShop.Contracts.CQ.Catalog;
using EShop.Contracts.CQ.Inventory.Responses;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Errors;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;

namespace Modules.Catalog.Features.Products.GetProductForCart;

public sealed class GetProductForCartHandler(
    CatalogDbContext context)
{
    public async Task<Result<ProductForCartResponse>> Handle(
        GetProductForCartQuery query,
        CancellationToken ct)
    {
        var product = await context.Products
            .AsNoTracking()
            .Where(x => x.Id == query.Id && x.IsActive)
            .Select(x => new ProductForCartResponse(
                x.Id,
                x.Price,
                x.IsActive))
            .SingleOrDefaultAsync(ct);

        if (product is null)
            return Result<ProductForCartResponse>.Failure(
                ProductErrors.NotFound);
        
        return Result<ProductForCartResponse>.Success(product);
    }
}