using Modules.Catalog.Application.Abstractions.IServices.IMediaServices;
using Modules.Catalog.Domain;
using Modules.Catalog.DTOs.Products;

namespace Modules.Catalog.Application.Mapping.Products;

public static class ProductMapper
{
    public static ProductDetailsResponse ToDetailsDto(
        this Product product,
        IMediaUrlService mediaUrlService,
        int availableQuantity)
    {
        return new ProductDetailsResponse(
            product.Id,
            product.Name,
            product.Description,
            mediaUrlService.GetUrl(product.MediaPath),
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
        IMediaUrlService mediaUrlService,
        int availableQuantity)
    {
        return new ProductListItemResponse(
            product.Id,
            product.Name,
            mediaUrlService.GetThumbnailUrl(product.ThumbnailPath),
            product.Price,
            product.CategoryId,
            product.BrandId,
            product.CreatedAt,
            availableQuantity
        );
    }
}