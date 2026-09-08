using EShop.Shared.Names;
using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.Application.Cache;
using Modules.Identity.Domain;
using Modules.Identity.Errors;
using Modules.Identity.Infrastructure.Persistence.Database.Context;

namespace Modules.Identity.Features.ConfirmEmail;

public sealed class ConfirmEmailHandler(
    IdentityDbContext context,
    ICacheService cacheService)
{
    public async Task<Result> Handle(
        ConfirmEmailCommand command,
        CancellationToken ct)
    {
        var cacheKey = $"temp_user:{command.Request.Email}";
        
        var tempUser = await cacheService.GetAsync<TempUser>(
            cacheKey, ct);

        if (tempUser is null)
            return Result.Failure(
                UserErrors.ConfirmationCodeNotFound);

        if (tempUser.ConfirmationCode != command.Request.Code)
            return Result.Failure(
                UserErrors.ConfirmationCodeInvalid);

        if (tempUser.CodeExpiresAt < DateTime.UtcNow)
            return Result.Failure(
                UserErrors.ConfirmationCodeExpired);

        var emailExist = await context.Users
            .AnyAsync(x => 
                x.Email == tempUser.Email, ct);

        if (emailExist)
            return Result.Failure(
                UserErrors.EmailExists);
        
        var customerRole = await context.Roles
            .SingleAsync(x =>
                x.Name == RoleNames.Customer, ct);

        var user = new User
        {
            Id = tempUser.Id,
            CreatedAt = tempUser.CreatedAt,
            Username = tempUser.Username,
            Email = tempUser.Email,
            PasswordHash = tempUser.PasswordHash,
            Roles = [customerRole]
        };

        await context.Users.AddAsync(user, ct);
        await context.SaveChangesAsync(ct);

        await cacheService.RemoveAsync(
            cacheKey, ct);

        return Result.Success();
    }
}