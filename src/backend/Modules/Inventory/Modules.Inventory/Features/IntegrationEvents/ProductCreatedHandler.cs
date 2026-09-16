using EShop.Contracts.Catalog.Products;
using Modules.Inventory.Domain;
using Modules.Inventory.Infrastructure.Persistence.Database.Context;
using Wolverine.Attributes;

namespace Modules.Inventory.Features.IntegrationEvents;

[Transactional(typeof(InventoryDbContext))]
public sealed class ProductCreatedHandler(
    InventoryDbContext context)
{
    public async Task Handle(
        ProductCreatedEvent @event, 
        CancellationToken ct)
    {
        var stock = new Stock
        {
            Id = Guid.CreateVersion7(),
            ProductId = @event.ProductId,
            Quantity = 0,
            ReservedQuantity = 0,
            CreatedAt = DateTime.UtcNow
        };

        await context.AddAsync(stock, ct);
    }
}