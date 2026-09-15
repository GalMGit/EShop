namespace EShop.Contracts.CQ.Catalog;

public sealed record GetProductsForOrderQuery(
    IReadOnlyCollection<Guid> ProductIds);