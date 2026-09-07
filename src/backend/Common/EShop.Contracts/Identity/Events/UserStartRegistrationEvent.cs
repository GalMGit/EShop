namespace EShop.Contracts.Identity.Events;

public sealed record UserStartRegistrationEvent(
    string Email, 
    string Username,
    string ConfirmationCode);