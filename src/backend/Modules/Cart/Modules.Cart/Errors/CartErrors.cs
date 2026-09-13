using EShop.Shared.ResultType;

namespace Modules.Cart.Errors;

public static class CartErrors
{
    public static readonly Error NotFound
        = Error.NotFound(
            "cart.not_found",
            "Корзина с таким Id не найдена.");
    
    public static readonly Error ItemNotFound
        = Error.NotFound(
            "cart.item_not_found",
            "Товар отсутствует в корзине");
}