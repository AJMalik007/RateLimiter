using RateLimiter.API.Services.RateLimit;

namespace RateLimiter.API.Middleware;

/// <summary>
/// Middleware for rate limiting requests.
/// </summary>
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly int _userRequestLimit;
    private readonly int _globalRequestLimit;
    private readonly TimeSpan _rateLimitExpiryTime;
    private readonly string? _globalClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="RateLimitingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="serviceScopeFactory">The service scope factory.</param>
    public RateLimitingMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
        _globalClient = Environment.GetEnvironmentVariable("GlobalClient");
        _userRequestLimit = int.Parse(Environment.GetEnvironmentVariable("UserRequestLimit") ?? "100");
        _globalRequestLimit = int.Parse(Environment.GetEnvironmentVariable("GlobalRequestLimit") ?? "1000");
        _rateLimitExpiryTime = TimeSpan.FromSeconds(int.Parse(Environment.GetEnvironmentVariable("RateLimitExpiryTime") ?? "60"));
    }

    /// <summary>
    /// Invokes the middleware to check rate limits.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var clientId = context.Request.Headers["X-API-KEY"].ToString();

        if (string.IsNullOrEmpty(clientId))
        {
            context.Response.StatusCode = 400; // Bad Request
            await context.Response.WriteAsJsonAsync(new { error = "X-API-KEY header missing." });
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();
        var rateLimitingService = scope.ServiceProvider.GetRequiredService<IRateLimitingService>();

        // Check client-specific rate limit
        if (!await rateLimitingService.IsRequestAllowedAsync($"clients:{clientId}", _userRequestLimit, _rateLimitExpiryTime))
        {
            context.Response.StatusCode = 429; // Too Many Requests
            context.Response.Headers.Add("Retry-After", _rateLimitExpiryTime.ToString());
            await context.Response.WriteAsJsonAsync(new { error = "Client request limit exceeded. Try again later." });
            return;
        }

        // Check global rate limit
        if (!await rateLimitingService.IsRequestAllowedAsync(_globalClient, _globalRequestLimit, _rateLimitExpiryTime))
        {
            context.Response.StatusCode = 429; // Too Many Requests
            context.Response.Headers.Add("Retry-After", _rateLimitExpiryTime.ToString());
            await context.Response.WriteAsJsonAsync(new { error = "Global request limit exceeded. Try again later." });
            return;
        }

        await _next(context);
    }
}




