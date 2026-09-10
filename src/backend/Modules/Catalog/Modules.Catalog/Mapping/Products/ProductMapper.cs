using Modules.Catalog.Domain;
using Modules.Catalog.DTOs.Products;

namespace Modules.Catalog.Mapping.Products;

public static class ProductMapper
{
    public static ProductDetailsResponse ToDetailsDto(
        this Product product,
        int availableQuantity)
    {
        return new ProductDetailsResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.CategoryId,
            product.BrandId,
            product.CreatedAt,
            product.Specifications,
            availableQuantity
            );
    }
    
    public static ProductListItemResponse ToListItemDto(
        this Product product,
        int availableQuantity)
    {
        return new ProductListItemResponse(
            product.Id,
            product.Name,
            null,
            product.Price,
            product.CategoryId,
            product.BrandId,
            product.CreatedAt,
            availableQuantity
        );
    }
}