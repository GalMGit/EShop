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
    public async Task<Result<OrderResponse>> Handle(
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
            return Result<OrderResponse>.Failure(
                OrderErrors.NotFound);

        return Result<OrderResponse>.Success(
            order.ToOrderResponse());
    }
}