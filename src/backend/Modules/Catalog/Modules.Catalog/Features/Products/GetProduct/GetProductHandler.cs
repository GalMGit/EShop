using EShop.Contracts.CQ.Inventory.Queries;
using EShop.Contracts.CQ.Inventory.Responses;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.DTOs.Products;
using Modules.Catalog.Errors;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;
using Modules.Catalog.Mapping.Products;
using Wolverine;

namespace Modules.Catalog.Features.Products.GetProduct;

public sealed class GetProductHandler(
    CatalogDbContext context,
    IMessageBus bus)
{
    public async Task<Result<ProductDetailsResponse>> Handle(
        GetProductQuery query,
        CancellationToken ct)
    {
        var product = await context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.Id, ct);

        if (product is null)
            return Result<ProductDetailsResponse>.Failure(
                ProductErrors.NotFound);

        var stockResult = await bus.InvokeAsync<
            Result<StockResponse>>(
                new GetStockQuery(product.Id), ct);

        if (stockResult.IsFailure)
            return Result<ProductDetailsResponse>.Failure(
                stockResult.Error!);

        return Result<ProductDetailsResponse>.Success(
            product.ToDetailsDto(stockResult.Value!.AvailableQuantity));
    }
}