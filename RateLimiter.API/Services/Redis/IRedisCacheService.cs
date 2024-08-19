namespace RateLimiter.API.Services.Redis;

public interface IRedisCacheService
{
    Task<string> GetCacheValueAsync(string key);
    Task SetCacheValueAsync(string key, string value, TimeSpan expiration);
}