using EShop.Shared.ResultType;

namespace Modules.Orders.Errors;

public static class OrderErrors
{
    public static readonly Error CartEmpty =
        Error.Validation(
            "orders.empty_cart",
            "Корзина пуста.");

    public static readonly Error ProductUnavailable =
        Error.Failure(
            "orders.product_unavailable",
            "Продукт недоступен");
    
    public static readonly Error NotFound =
        Error.NotFound(
            "orders.not_found",
            "Заказ не найден.");
}