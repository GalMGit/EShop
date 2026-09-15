namespace Modules.Orders.DTOs;

public sealed record CheckoutResponse(
    Guid OrderId, 
    decimal TotalAmount);
    
