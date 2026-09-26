using Modules.Catalog.Infrastructure.Search;

namespace Modules.Catalog.Application.DTOs.Products;

public sealed record SearchProductsResponse(
    IReadOnlyCollection<ProductSearchDocument> Items,
    long TotalCount);