namespace EShop.Contracts.Events.Identity.Events;

public sealed record UserCreatedEvent(
    Guid UserId);