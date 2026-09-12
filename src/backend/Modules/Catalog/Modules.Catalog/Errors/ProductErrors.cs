using EShop.Shared.ResultType;

namespace Modules.Catalog.Errors;

public static class ProductErrors
{
    public static readonly Error NotFound =
        Error.NotFound(
            "catalog.product_not_found",
            "Товар не найден.");
    
    public static readonly Error SearchFailed =
        Error.Failure(
            "catalog.search_failure",
            "Не удалось выполнить поиск товаров.");
}