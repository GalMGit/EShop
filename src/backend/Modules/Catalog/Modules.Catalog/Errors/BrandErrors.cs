using EShop.Shared.ResultType;

namespace Modules.Catalog.Errors;

public static class BrandErrors
{
    public static readonly Error BrandExists =
        Error.Conflict(
            "catalog.brand_exists",
            "Бренд с таким названием уже существует.");
    
    public static readonly Error NotFound =
        Error.NotFound(
            "catalog.brand_not_found",
            "Бренда с таким id не существует.");
}