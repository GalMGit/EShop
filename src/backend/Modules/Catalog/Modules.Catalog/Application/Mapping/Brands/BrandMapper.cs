using Modules.Catalog.Application.DTOs.Brands;
using Modules.Catalog.Domain;

namespace Modules.Catalog.Application.Mapping.Brands;

public static class BrandMapper
{
    public static BrandResponse ToDto(this Brand brand)
    {
        return new BrandResponse(
            brand.Id,
            brand.Name);
    }
}