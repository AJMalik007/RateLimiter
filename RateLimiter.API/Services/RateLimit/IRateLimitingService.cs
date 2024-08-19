namespace RateLimiter.API.Services.RateLimit;

public interface IRateLimitingService
{
    Task<bool> IsRequestAllowedAsync(string clientId, int maxRequests, TimeSpan timePeriod);
}