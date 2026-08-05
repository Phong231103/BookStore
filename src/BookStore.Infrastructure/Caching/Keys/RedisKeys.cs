using BookStore.Domain.Users.ValueObjects;

namespace BookStore.Infrastructure.Caching.Keys;

internal static class RedisKeys
{
    private const string RegistrationPrefix = "registration";

    private const string RegistrationEmailPrefix = "registration-email";

    public static string Registration(Guid registrationId)
        => $"{RegistrationPrefix}:{registrationId}";

    public static string RegistrationEmail(Email email)
        => $"{RegistrationEmailPrefix}:{email.Value}";
}