namespace EShop.Contracts.CQ.Catalog;

public sealed record GetProductsForCartQuery(
    IReadOnlyCollection<Guid> ProductIds);