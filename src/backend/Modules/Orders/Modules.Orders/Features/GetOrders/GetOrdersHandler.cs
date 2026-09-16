using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Orders.Application.Mapping;
using Modules.Orders.DTOs;
using Modules.Orders.Infrastructure.Persistence.Database.Context;

namespace Modules.Orders.Features.GetOrders;

public sealed class GetOrdersHandler(
    OrderDbContext context)
{
    public async Task<Result<List<OrderResponse>>> Handle(
        GetOrdersQuery query,
        CancellationToken ct)
    {
        var orders = await context.Orders
            .AsNoTracking()
            .Where(x => 
                x.UserId == query.UserId)
            .ToListAsync(ct);

        return Result<List<OrderResponse>>.Success(
            orders
                .Select(x => 
                    x.ToOrderResponse())
                .ToList());
    }
}