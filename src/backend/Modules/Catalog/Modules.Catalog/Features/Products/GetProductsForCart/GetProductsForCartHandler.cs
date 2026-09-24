using EShop.Contracts.CQ.Catalog;
using EShop.Contracts.CQ.Catalog.Responses;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Application.Abstractions.IServices.IMediaServices;
using Modules.Catalog.Errors;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;

namespace Modules.Catalog.Features.Products.GetProductsForCart;

public sealed class GetProductsForCartHandler(
    CatalogDbContext context,
    IMediaUrlService mediaUrlService)
{
    public async Task<List<ProductForCartResponse>> Handle(
        GetProductsForCartQuery query,
        CancellationToken ct)
    {
        var products = await context.Products
            .AsNoTracking()
            .Where(x => query.ProductIds
                .Contains(x.Id))
            .Select(x => new ProductForCartResponse(
                x.Id,
                mediaUrlService.GetThumbnailUrl(
                    x.ThumbnailPath),
                x.Name,
                x.Price,
                x.IsActive))
            .ToListAsync(ct);
        
        return products;
    }
}