namespace EShop.Contracts.CQ.Cart.Commands;

public sealed record ClearCartCommand(
    Guid UserId);