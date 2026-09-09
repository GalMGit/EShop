using Modules.Catalog.Domain;
using Modules.Catalog.DTOs.Products;

namespace Modules.Catalog.Mapping.Products;

public static class ProductMapper
{
    public static ProductResponse ToDto(
        this Product product)
    {
        return new ProductResponse(product.Name,
            product.Description,
            product.Price,
            product.CategoryId,
            product.BrandId,
            product.CreatedAt,
            product.Specifications);
    }
}