using Elastic.Clients.Elasticsearch;
using EShop.Contracts.Catalog.Products;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Infrastructure.Persistence.Database.Context;
using Modules.Catalog.Infrastructure.Search;

namespace Modules.Catalog.Features.IntegrationEvents.ProductCreated;

public sealed class ProductCreatedHandler(
    CatalogDbContext context,
    ElasticsearchClient elasticsearch)
{
    public async Task Handle(
        ProductCreatedEvent @event,
        CancellationToken ct)
    {
        var product = await context.Products
            .AsNoTracking()
            .SingleAsync(
                x => x.Id == @event.ProductId,
                ct);

        var document = new ProductSearchDocument
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            BrandId = product.BrandId,
            IsActive = product.IsActive,
            Specifications = product.Specifications
        };

        await elasticsearch.IndexAsync(
            document,
            x => x
                .Index("products")
                .Id(document.Id),
            ct);
    }
}