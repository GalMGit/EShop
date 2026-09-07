namespace Modules.Identity.Features.CreateUser;

public sealed record CreateUserRequest(
    string Username,
    string Password);