using System.Security.Cryptography;
using EShop.Contracts.Identity.Events;
using EShop.Shared.Names;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.Application.Abstractions.Auth;
using Modules.Identity.Application.Cache;
using Modules.Identity.Domain;
using Modules.Identity.Errors;
using Modules.Identity.Infrastructure.Persistence.Database.Context;
using Wolverine;

namespace Modules.Identity.Features.CreateUser;

public sealed class CreateUserHandler(
    IdentityDbContext context,
    ICacheService cacheService,
    IMessageBus bus,
    IPasswordHasher passwordHasher)
{
    public async Task<Result> Handle(
        CreateUserCommand command,
        CancellationToken ct)
    {
        var cacheKey = $"temp_user:{command.Request.Email}";
        
        if (await cacheService.ExistsAsync(
                cacheKey, ct))
            return Result.Failure(UserErrors.RegistrationPending);
        
        var confirmationCode =
            RandomNumberGenerator
                .GetInt32(10000, 100000)
                .ToString();
        
        var customerRole = await context.Roles
            .SingleAsync(x =>
                x.Name == RoleNames.Customer, ct);
        
        var tempUser = new TempUser
        {
            Id = Guid.CreateVersion7(),
            Username = command.Request.Username,
            Email = command.Request.Email,
            PasswordHash = passwordHasher.GenerateHash(
                command.Request.Password),
            CreatedAt = DateTime.UtcNow,
            Roles = [customerRole],
            ConfirmationCode = confirmationCode,
            CodeExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };
        
        await cacheService.SetAsync(
            cacheKey,
            tempUser,
            TimeSpan.FromMinutes(5),
            ct);
        
        await bus.PublishAsync(
            new UserStartRegistrationEvent(
                tempUser.Email,
                tempUser.Username,
                tempUser.ConfirmationCode));
        
        return Result.Success();
    }
}