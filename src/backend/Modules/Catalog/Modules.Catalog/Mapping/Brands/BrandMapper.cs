using Modules.Catalog.Domain;
using Modules.Catalog.DTOs.Brands;

namespace Modules.Catalog.Mapping.Brands;

public static class BrandMapper
{
    public static BrandResponse ToDto(this Brand brand)
    {
        return new BrandResponse(
            brand.Id,
            brand.Name);
    }
}