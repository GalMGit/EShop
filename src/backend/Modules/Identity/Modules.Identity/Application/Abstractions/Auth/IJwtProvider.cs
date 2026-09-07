using Modules.Identity.Domain;

namespace Modules.Identity.Application.Abstractions.Auth;

public interface IJwtProvider
{
    string GenerateRefreshToken();
    string GenerateToken(User user);
}