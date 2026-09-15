namespace EShop.Contracts.CQ.Cart.Responses;

public sealed record CartItemForCheckoutResponse(
    Guid ProductId,
    int Quantity);