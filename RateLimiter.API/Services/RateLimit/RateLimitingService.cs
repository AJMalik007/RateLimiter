using StackExchange.Redis;

namespace RateLimiter.API.Services.RateLimit;

public class RateLimitingService : IRateLimitingService
{
    private readonly IConnectionMultiplexer _redis;

    public RateLimitingService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<bool> IsRequestAllowedAsync(string clientId, int maxRequests, TimeSpan timePeriod)
    {
        var db = _redis.GetDatabase();
        var currentRequestCount = await db.StringIncrementAsync(clientId);

        if (currentRequestCount == 1)
        {
            await db.KeyExpireAsync(clientId, timePeriod);
        }

        return currentRequestCount <= maxRequests;
    }
}