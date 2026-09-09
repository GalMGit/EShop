using EShop.Shared.ResultType;

namespace Modules.Catalog.Errors;

public static class ProductErrors
{
    public static readonly Error NotFound =
        Error.NotFound(
            "catalog.product_not_found",
            "Товар не найден.");
}