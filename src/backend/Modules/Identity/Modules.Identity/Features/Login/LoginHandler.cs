using EShop.Shared.ResultType;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.Application.Abstractions.Auth;
using Modules.Identity.Errors;
using Modules.Identity.Infrastructure.Persistence.Database.Context;

namespace Modules.Identity.Features.Login;

public sealed class LoginHandler(
    IdentityDbContext context,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider)
{
    public async Task<Result<LoginResponse>> Handle(
        LoginCommand command,
        CancellationToken ct)
    {
        var user = await context.Users
            .Include(x => x.Roles)
            .ThenInclude(p => p.Permissions)
            .FirstOrDefaultAsync(x => 
                x.Email == command.Request.Email, ct);

        if (user is null)
            return Result<LoginResponse>.Failure(
                UserErrors.NotFound);

        var token = jwtProvider.GenerateToken(user);

        return Result<LoginResponse>.Success(
            new LoginResponse(token));
    }
}