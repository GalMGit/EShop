using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using EShop.Shared.ResultType;
using Modules.Catalog.DTOs.Products;
using Modules.Catalog.Errors;
using Modules.Catalog.Infrastructure.Search;

namespace Modules.Catalog.Features.Products.SearchProducts;

public sealed class SearchProductsHandler(
    ElasticsearchClient elasticsearch)
{
    public async Task<Result<SearchProductsResponse>> Handle(
        SearchProductsQuery query,
        CancellationToken ct)
    {
        var response = await elasticsearch.SearchAsync<ProductSearchDocument>(
            s => s
                .Indices("products")
                .From((query.Page - 1) * query.PageSize)
                .Size(query.PageSize)
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Fields(
                            f => f.Name,
                            f => f.Description)
                        .Type(TextQueryType.BoolPrefix)
                        .Query(query.Search))),
            ct);

        if (!response.IsValidResponse)
        {
            return Result<SearchProductsResponse>.Failure(
                ProductErrors.SearchFailed);
        }

        var products = response.Hits
            .Select(x => x.Source!)
            .ToList();

        return Result<SearchProductsResponse>.Success(
            new SearchProductsResponse(
                products,
                (int)response.Total));
    }
}