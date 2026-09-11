using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Inventory.Errors;
using Modules.Inventory.Infrastructure.Persistence.Database.Context;

namespace Modules.Inventory.Features.AddStock;

public sealed class AddStockHandler(
    InventoryDbContext context)
{
    public async Task<Result> Handle(
        AddStockCommand command,
        CancellationToken ct)
    {
        var stock = await context.Stocks
            .SingleOrDefaultAsync(x =>
                x.ProductId == command.Request.ProductId, ct);

        if (stock is null)
            return Result.Failure(
                StockErrors.NotFound);

        stock.Quantity += command.Request.Quantity;
        stock.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync(ct);

        return Result.Success();
    }
}