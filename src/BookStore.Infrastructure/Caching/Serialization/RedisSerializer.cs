using StackExchange.Redis;
using System.Text.Json;

namespace BookStore.Infrastructure.Caching.Serialization;

internal static class RedisSerializer
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, Options);
    }

    public static T? Deserialize<T>(RedisValue value)
    {
        if (value.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(value, Options);
    }
}