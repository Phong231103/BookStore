using BookStore.Domain.Common.Services;
using Microsoft.Extensions.Options;

namespace BookStore.Infrastructure.Security;

public sealed class BcryptPasswordHasher : IPasswordHasher
{
    private readonly PasswordHashOptions _options;

    public BcryptPasswordHasher(IOptions<PasswordHashOptions> options)
    {
        _options = options.Value;
    }

    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: _options.WorkFactor);
    }

    public bool Verify(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
