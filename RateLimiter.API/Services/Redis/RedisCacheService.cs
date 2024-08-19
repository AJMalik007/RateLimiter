using StackExchange.Redis;

namespace RateLimiter.API.Services.Redis;

public class RedisCacheService : IRedisCacheService
{
    private readonly IConnectionMultiplexer _redis;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<string> GetCacheValueAsync(string key)
    {
        var db = _redis.GetDatabase();
        return await db.StringGetAsync(key);
    }

    public async Task SetCacheValueAsync(string key, string value, TimeSpan expiration)
    {
        var db = _redis.GetDatabase();
        await db.StringSetAsync(key, value, expiration);
    }
}