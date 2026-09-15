namespace EShop.Contracts.CQ.Cart.Responses;

public sealed record CartForCheckoutResponse(
    IReadOnlyCollection<CartItemForCheckoutResponse> Items);