namespace RateLimiter.API.Services.Redis;

/// <summary>
/// Interface for Redis cache service.
/// </summary>
public interface IRedisCacheService
{
    /// <summary>
    /// Asynchronously gets the cached value for the specified key.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cached value as a string.</returns>
    Task<string> GetCacheValueAsync(string key);

    /// <summary>
    /// Asynchronously sets the cache value for the specified key with an expiration time.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="expiration">The expiration time for the cache entry.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetCacheValueAsync(string key, string value, TimeSpan expiration);
}



