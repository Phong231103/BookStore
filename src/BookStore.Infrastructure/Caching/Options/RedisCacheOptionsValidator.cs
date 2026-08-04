using Microsoft.Extensions.Options;

namespace BookStore.Infrastructure.Caching.Options;

internal sealed class RedisCacheOptionsValidator
    : IValidateOptions<RedisCacheOptions>
{
    public ValidateOptionsResult Validate(
        string? name,
        RedisCacheOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            return ValidateOptionsResult.Fail(
                "Redis connection string is required.");
        }

        if (options.Database < 0)
        {
            return ValidateOptionsResult.Fail(
                "Redis database must be greater than or equal to zero.");
        }

        if (options.RegistrationExpirationMinutes <= 0)
        {
            return ValidateOptionsResult.Fail(
                "Registration expiration must be greater than zero.");
        }

        return ValidateOptionsResult.Success;
    }
}