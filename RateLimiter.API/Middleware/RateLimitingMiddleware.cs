using RateLimiter.API.Services.RateLimit;

namespace RateLimiter.API.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly int _userRequestLimit;
    private readonly int _globalRequestLimit;
    private readonly TimeSpan _rateLimitExpiryTime;
    private readonly string? _globalClient;

    public RateLimitingMiddleware(
        RequestDelegate next,
        IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
        _globalClient = Environment.GetEnvironmentVariable("GlobalClient");
        _userRequestLimit = Convert.ToInt32(Environment.GetEnvironmentVariable("UserRequestLimit"));
        _globalRequestLimit = Convert.ToInt32(Environment.GetEnvironmentVariable("GlobalRequestLimit"));
        _rateLimitExpiryTime = TimeSpan.FromSeconds(Convert.ToInt32(Environment.GetEnvironmentVariable("RateLimitExpiryTime")));
    }

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

        var clientLimitExceeded = !await rateLimitingService.IsRequestAllowedAsync(
            $"clients:{clientId}",
            _userRequestLimit,
            _rateLimitExpiryTime);

        if (clientLimitExceeded)
        {
            // Too Many Requests
            context.Response.StatusCode = 429;
            context.Response.Headers.Add("Retry-After", _rateLimitExpiryTime.ToString());
            await context.Response.WriteAsJsonAsync(new { error = "Client request limit exceeded. Try again later." });
            return;
        }

        var globalLimitExceeded = !await rateLimitingService.IsRequestAllowedAsync(
            _globalClient,
            _globalRequestLimit,
            _rateLimitExpiryTime);

        if (globalLimitExceeded)
        {
            // Too Many Requests
            context.Response.StatusCode = 429;
            context.Response.Headers.Add("Retry-After", _rateLimitExpiryTime.ToString());
            await context.Response.WriteAsJsonAsync(new { error = "Global request limit exceeded. Try again later." });
            return;
        }

        await _next(context);
    }
}
