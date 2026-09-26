namespace Modules.Orders.Application.DTOs;

public sealed record CheckoutResponse(
    Guid OrderId, 
    decimal TotalAmount);
    
