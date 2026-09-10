using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.Errors;
using Modules.Identity.Infrastructure.Persistence.Database.Context;

namespace Modules.Identity.Features.ChangeRole;

public sealed class ChangeRoleHandler(
    IdentityDbContext context)
{
    public async Task<Result> Handle(
        ChangeRoleCommand command, 
        CancellationToken ct)
    {
        var role = await context.Roles
            .SingleOrDefaultAsync(x => 
                x.Id == command.Request.RoleId, ct);

        if (role is null)
            return Result.Failure(
                RoleErrors.NotFound);

        var user = await context.Users
            .SingleOrDefaultAsync(x =>
                x.Id == command.Request.UserId, ct);

        if (user is null)
            return Result.Failure(
                UserErrors.NotFound);
        
        user.Roles.Add(role);
        await context.SaveChangesAsync(ct);

        return Result.Success();
    }
}