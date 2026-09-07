using EShop.Shared.Names;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.Application.Abstractions.Auth;
using Modules.Identity.Domain;
using Modules.Identity.Infrastructure.Persistence.Database.Context;

namespace Modules.Identity.Features.CreateUser;

public sealed class CreateUserHandler(
    IdentityDbContext context,
    IPasswordHasher passwordHasher)
{
    public async Task<Result> Handle(
        CreateUserCommand command,
        CancellationToken ct)
    {
        var customerRole = await context.Roles
            .SingleAsync(x =>
                x.Name == RoleNames.Customer, ct);
        
        var user = new User
        {
            Id = Guid.CreateVersion7(),
            Username = command.Request.Username,
            CreatedAt = DateTime.UtcNow,
            Roles = [customerRole],
            Email = command.Request.Email,
            PasswordHash = passwordHasher.GenerateHash(
                command.Request.Password)
        };

        await context.Users.AddAsync(user, ct);
        await context.SaveChangesAsync(ct);
        
        return Result.Success();
    }
}