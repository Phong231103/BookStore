namespace BookStore.Infrastructure.Caching.Options
{
    public sealed class RedisCacheOptions
    {
        public const string SectionName = "Redis";

        public string ConnectionString { get; init; } = string.Empty;

        public int Database { get; init; }

        public required RedisExpirationOptions Expirations { get; init; }
    }
}
