using Modules.Catalog.Domain;
using Modules.Catalog.DTOs.Categories;

namespace Modules.Catalog.Mapping.Categories;

public static class CategoryMapper
{
    public static CategoryResponse ToDto(
        this Category category)
    {
        return new CategoryResponse(
            category.Name,
            category.Id,
            category.ParentId);
    }
}