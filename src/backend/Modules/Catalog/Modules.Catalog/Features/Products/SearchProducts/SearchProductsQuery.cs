namespace Modules.Catalog.Features.Products.SearchProducts;

public sealed record SearchProductsQuery(
    string Search,
    int Page = 1,
    int PageSize = 20);