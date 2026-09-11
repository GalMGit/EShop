using Modules.Catalog.Infrastructure.Search;

namespace Modules.Catalog.DTOs.Products;

public sealed record SearchProductsResponse(
    IReadOnlyCollection<ProductSearchDocument> Items,
    long TotalCount);