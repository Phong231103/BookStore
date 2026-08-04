namespace BookStore.Infrastructure.Caching.Keys;

internal static class RedisKeys
{
    private const string RegistrationPrefix = "registration";

    public static string Registration(Guid registrationId)
        => $"{RegistrationPrefix}:{registrationId}";
}