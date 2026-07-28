namespace BookStore.Domain.Common.Services;

/// <summary>
/// Provides password hashing and verification services.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes the specified plain text password.
    /// </summary>
    string Hash(string password);

    /// <summary>
    /// Verifies whether the specified password matches the stored hash.
    /// </summary>
    bool Verify(string password, string passwordHash);
}