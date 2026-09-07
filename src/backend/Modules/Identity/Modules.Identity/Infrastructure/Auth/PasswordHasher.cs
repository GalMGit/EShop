using Microsoft.AspNetCore.Identity;
using Modules.Identity.Application.Abstractions.Auth;

namespace Modules.Identity.Infrastructure.Auth;

public sealed class PasswordHasher : IPasswordHasher
{
    public string GenerateHash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyHash(
        string password,
        string hashedPassword)
        => BCrypt.Net.BCrypt.Verify(
            password,
            hashedPassword);
}