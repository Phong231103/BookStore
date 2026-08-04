using BookStore.Application.Registration.Common;
using BookStore.Application.Registration.Interfaces;
using BookStore.Infrastructure.Caching.Keys;
using BookStore.Infrastructure.Caching.Options;
using BookStore.Infrastructure.Caching.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace BookStore.Infrastructure.Caching.Registration
{
    public sealed class RedisPendingRegistrationStore : IPendingRegistrationStore
    {
        private readonly IConnectionMultiplexer _connection;

        private readonly RedisCacheOptions _options;

        private readonly ILogger<RedisPendingRegistrationStore> _logger;



        public RedisPendingRegistrationStore(IConnectionMultiplexer connection, IOptions<RedisCacheOptions> options, ILogger<RedisPendingRegistrationStore> logger)
        {
            _connection = connection;

            _options = options.Value;

            _logger = logger;
        }

        private IDatabase Database => _connection.GetDatabase(_options.Database);

        public async Task SaveAsync(PendingRegistration registration, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentNullException.ThrowIfNull(registration);

            RedisKey key = RedisKeys.Registration(registration.RegistrationId);

            RedisValue value = RedisSerializer.Serialize(registration);

            TimeSpan ttl = TimeSpan.FromMinutes(_options.RegistrationExpirationMinutes);

            bool success = await Database.StringSetAsync(key, value, ttl);

            if (!success)
            {
                _logger.LogError("Failed to save registration cache. RegistrationId: {RegistrationId}", registration.RegistrationId);

                throw new InvalidOperationException("Failed to save registration cache.");
            }
        }

        public async Task<PendingRegistration?> GetAsync(Guid registrationId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            RedisKey key = RedisKeys.Registration(registrationId);

            RedisValue value = await Database.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                return null;
            }

            PendingRegistration? registration = RedisSerializer.Deserialize<PendingRegistration>(value);

            return registration;
        }

        public async Task RemoveAsync(Guid registrationId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            RedisKey key = RedisKeys.Registration(registrationId);

            bool success = await Database.KeyDeleteAsync(key);

            if (!success)
            {
                _logger.LogWarning("Registration cache not found. RegistrationId: {RegistrationId}", registrationId);
            }
        }
    }
}
