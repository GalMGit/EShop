using EShop.Contracts.CQ.Inventory.Queries;
using EShop.Contracts.CQ.Inventory.Responses;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Inventory.Errors;
using Modules.Inventory.Infrastructure.Persistence.Database.Context;

namespace Modules.Inventory.Features.Stocks.GetStock;

public sealed class GetStockHandler(
    InventoryDbContext context)
{
    public async Task<Result<StockResponse>> Handle(
        GetStockQuery query, 
        CancellationToken ct)
    {
        var stock = await context.Stocks
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.ProductId == query.ProductId,
                ct);
        
        if (stock is null)
            return Result<StockResponse>.Failure(
                StockErrors.NotFound);
        
        return Result<StockResponse>.Success(
            new StockResponse(
                stock.ProductId,
                stock.AvailableQuantity));
    }
}