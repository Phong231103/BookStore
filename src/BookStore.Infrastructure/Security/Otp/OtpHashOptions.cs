namespace BookStore.Infrastructure.Security.Otp;

public sealed class OtpHashOptions
{
    public const string SectionName = "Otp";

    public required string SecretKey { get; init; }
}