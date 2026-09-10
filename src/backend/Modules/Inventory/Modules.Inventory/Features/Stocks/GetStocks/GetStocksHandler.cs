using EShop.Contracts.CQ.Inventory.Queries;
using EShop.Contracts.CQ.Inventory.Responses;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Inventory.Infrastructure.Persistence.Database.Context;

namespace Modules.Inventory.Features.Stocks.GetStocks;

public sealed class GetStocksHandler(
    InventoryDbContext context)
{
    public async Task<Result<List<StockResponse>>> Handle(
        GetStocksQuery query,
        CancellationToken ct)
    {
        var stocks = await context.Stocks
            .AsNoTracking()
            .Where(x => query.ProductIds
                .Contains(x.ProductId))
            .Select(x => new StockResponse(
                x.ProductId,
                x.AvailableQuantity))
            .ToListAsync(ct);

        return Result<List<StockResponse>>.Success(stocks);
    }
}