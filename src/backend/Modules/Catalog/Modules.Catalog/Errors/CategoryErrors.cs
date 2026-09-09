using EShop.Shared.ResultType;

namespace Modules.Catalog.Errors;

public static class CategoryErrors
{
    public static readonly Error CategoryExists = 
        Error.Conflict(
            "catalog.category_exists", 
            "Такая категория уже существует.");
    
    public static readonly Error ParentNotFound = 
        Error.Conflict(
            "catalog.parent_category_not_found", 
            "Родительской категории с таким Id не существует.");
    
    public static readonly Error NotFound = 
        Error.NotFound(
            "catalog.category_not_found", 
            "Категории с таким Id не существует.");
}