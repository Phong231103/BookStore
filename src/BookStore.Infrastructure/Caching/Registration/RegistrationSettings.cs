using BookStore.Application.Registrations.Interfaces;
using BookStore.Infrastructure.Caching.Options;
using Microsoft.Extensions.Options;

namespace BookStore.Infrastructure.Caching.Registration
{
    internal sealed class RegistrationSettings : IRegistrationSettings
    {
        public TimeSpan Expiration { get; }

        public RegistrationSettings(
            IOptions<RedisCacheOptions> options)
        {
            Expiration = TimeSpan.FromMinutes(
                options.Value.RegistrationExpirationMinutes);
        }
    }
}
