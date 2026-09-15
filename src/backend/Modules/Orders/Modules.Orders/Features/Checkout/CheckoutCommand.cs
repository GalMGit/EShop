namespace Modules.Orders.Features.Checkout;

public sealed record CheckoutCommand(
    Guid UserId);