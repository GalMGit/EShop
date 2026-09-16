using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Orders.Application.Mapping;
using Modules.Orders.DTOs;
using Modules.Orders.Errors;
using Modules.Orders.Infrastructure.Persistence.Database.Context;

namespace Modules.Orders.Features.GetOrder;

public sealed class GetOrderHandler(
    OrderDbContext context)
{
    public async Task<Result<OrderWithItemsResponse>> Handle(
        GetOrderQuery query,
        CancellationToken ct)
    {
        var order = await context.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .Where(x => 
                x.Id == query.OrderId &&
                x.UserId == query.UserId)
            .SingleOrDefaultAsync(ct);

        if (order is null)
            return Result<OrderWithItemsResponse>.Failure(
                OrderErrors.NotFound);

        return Result<OrderWithItemsResponse>.Success(
            order.ToOrderWithItemsResponse());
    }
}