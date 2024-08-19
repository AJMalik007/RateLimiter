using StackExchange.Redis;

namespace RateLimiter.API.Services.RateLimit;

/// <summary>
/// Service for rate limiting requests.
/// </summary>
public class RateLimitingService : IRateLimitingService
{
    private readonly IConnectionMultiplexer _redis;

    /// <summary>
    /// Initializes a new instance of the <see cref="RateLimitingService"/> class.
    /// </summary>
    /// <param name="redis">The Redis connection multiplexer.</param>
    public RateLimitingService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    /// <summary>
    /// Asynchronously checks if a request is allowed based on the rate limit.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="maxRequests">The maximum number of allowed requests.</param>
    /// <param name="timePeriod">The time period for the rate limit.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the request is allowed.</returns>
    public async Task<bool> IsRequestAllowedAsync(string clientId, int maxRequests, TimeSpan timePeriod)
    {
        var db = _redis.GetDatabase();
        var currentRequestCount = await db.StringIncrementAsync(clientId);

        // Set the expiration time for the key if this is the first request
        if (currentRequestCount == 1)
        {
            await db.KeyExpireAsync(clientId, timePeriod);
        }

        // Check if the current request count is within the allowed limit
        return currentRequestCount <= maxRequests;
    }
}


