namespace BookStore.Infrastructure.Security;

public sealed class PasswordHashOptions
{
    public const string SectionName = "PasswordHashing";

    public int WorkFactor { get; init; } = 12;
}
