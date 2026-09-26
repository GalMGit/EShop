using Modules.Catalog.Application.DTOs.Categories;
using Modules.Catalog.Domain;

namespace Modules.Catalog.Application.Mapping.Categories;

public static class CategoryMapper
{
    public static CategoryResponse ToDto(
        this Category category)
    {
        return new CategoryResponse(
            category.Name,
            category.Id);
    }
}