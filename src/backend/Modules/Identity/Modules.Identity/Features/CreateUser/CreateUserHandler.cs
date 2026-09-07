using Microsoft.AspNetCore.Identity;
using Modules.Identity.Application.Abstractions.Auth;
using Modules.Identity.Domain;
using Modules.Identity.Infrastructure.Persistence.Database.Context;

namespace Modules.Identity.Features.CreateUser;

public sealed class CreateUserHandler(
    IdentityDbContext context,
    IPasswordHasher passwordHasher)
{
    public async Task Handle(
        CreateUserCommand command,
        CancellationToken ct)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = command.Request.Username,
            CreatedAt = DateTime.UtcNow,
            Email = "example@gmail.com",
            PasswordHash = passwordHasher.GenerateHash(
                command.Request.Password)
        };

        await context.Users.AddAsync(user, ct);
        await context.SaveChangesAsync(ct);
    }
}