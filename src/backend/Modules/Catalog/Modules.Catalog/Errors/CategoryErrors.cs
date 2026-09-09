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
}