namespace RateLimiter.API.Services.RateLimit;
/// <summary>
/// Interface for rate limiting service.
/// </summary>
public interface IRateLimitingService
{
    /// <summary>
    /// Asynchronously checks if a request is allowed based on the rate limit.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="maxRequests">The maximum number of allowed requests.</param>
    /// <param name="timePeriod">The time period for the rate limit.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the request is allowed.</returns>
    Task<bool> IsRequestAllowedAsync(string clientId, int maxRequests, TimeSpan timePeriod);
}