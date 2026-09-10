using EShop.Contracts.CQ.Inventory.Queries;
using EShop.Contracts.CQ.Inventory.Responses;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.DTOs.Products;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;
using Modules.Catalog.Mapping.Products;
using Wolverine;

namespace Modules.Catalog.Features.Products.GetProducts;

public sealed class GetProductsHandler(
    CatalogDbContext context,
    IMessageBus bus)
{
    public async Task<Result<List<ProductListItemResponse>>> Handle(
        GetProductsQuery query,
        CancellationToken ct)
    {
        var products = await context.Products
            .AsNoTracking()
            .ToListAsync(ct);

        var productIds = products
            .Select(x => x.Id)
            .ToList();

        var stocksResult = await bus.InvokeAsync<
            Result<List<StockResponse>>>(
                new GetStocksQuery(
                    productIds), ct);

        if (stocksResult.IsFailure)
            return Result<List<ProductListItemResponse>>.Failure(
                stocksResult.Error!);

        var stocks = stocksResult.Value!
            .ToDictionary(x => x.ProductId);

        var result = products
            .Select(product =>
            {
                var availableQuantity = stocks
                    .TryGetValue(
                        product.Id,
                        out var stock)
                    ? stock.AvailableQuantity
                    : 0;

                return product.ToListItemDto(availableQuantity);
            })
            .ToList();

        return Result<List<ProductListItemResponse>>.Success(result);
    }
}