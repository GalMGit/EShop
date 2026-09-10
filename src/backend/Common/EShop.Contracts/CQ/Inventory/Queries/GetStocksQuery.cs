namespace EShop.Contracts.CQ.Inventory.Queries;

public sealed record GetStocksQuery(
    IReadOnlyCollection<Guid> ProductIds);