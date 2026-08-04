namespace BookStore.Infrastructure.Caching.Options
{
    public sealed class RedisExpirationOptions
    {
        public int Registration { get; init; } = 5;
    }
}
