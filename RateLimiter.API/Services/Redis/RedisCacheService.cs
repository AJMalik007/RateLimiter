using StackExchange.Redis;

namespace RateLimiter.API.Services.Redis;

/// <summary>
/// Service for interacting with Redis cache.
/// </summary>
public class RedisCacheService : IRedisCacheService
{
    private readonly IConnectionMultiplexer _redis;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisCacheService"/> class.
    /// </summary>
    /// <param name="redis">The Redis connection multiplexer.</param>
    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    /// <summary>
    /// Asynchronously gets the cached value for the specified key.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cached value as a string.</returns>
    public async Task<string> GetCacheValueAsync(string key)
    {
        var db = _redis.GetDatabase();
        return await db.StringGetAsync(key);
    }

    /// <summary>
    /// Asynchronously sets the cache value for the specified key with an expiration time.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="expiration">The expiration time for the cache entry.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetCacheValueAsync(string key, string value, TimeSpan expiration)
    {
        var db = _redis.GetDatabase();
        await db.StringSetAsync(key, value, expiration);
    }
}




