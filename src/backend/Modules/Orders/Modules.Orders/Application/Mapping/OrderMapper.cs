using Modules.Orders.Domain;
using Modules.Orders.DTOs;

namespace Modules.Orders.Application.Mapping;

public static class OrderMapper
{
    public static OrderResponse ToOrderResponse(
        this Order order)
    {
        return new OrderResponse(
            order.Id,
            order.TotalAmount,
            order.Status,
            order.CreatedAt,
            order.Items
                .Select(x => 
                    x.ToOrderItemResponse())
                .ToList()
        );
    }

    public static OrderItemResponse ToOrderItemResponse(
        this OrderItem orderItem)
    {
        return new OrderItemResponse(
            orderItem.Id,
            orderItem.ProductId,
            orderItem.ProductName,
            orderItem.ProductImageUrl,
            orderItem.Quantity,
            orderItem.UnitPrice,
            orderItem.TotalPrice);
    }
}