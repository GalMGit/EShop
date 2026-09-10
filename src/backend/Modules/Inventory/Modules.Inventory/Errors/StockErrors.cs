using EShop.Shared.ResultType;

namespace Modules.Inventory.Errors;

public static class StockErrors
{
    public static readonly Error NotFound =
        Error.NotFound(
            "inventory.stock_not_found",
            "Остаток не найден");
}