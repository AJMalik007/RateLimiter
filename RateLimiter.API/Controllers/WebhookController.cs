using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RateLimiter.API.Common;
using RateLimiter.API.Mock;
using RateLimiter.API.Services.Redis;

namespace RateLimiter.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WebhookController : ControllerBase
{
    private readonly IRedisCacheService _redisCache;
    private readonly IExternalService _externalService;
    private readonly int _cacheExpiryTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataController"/> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    public WebhookController(
        IRedisCacheService redisCache,
        IExternalService externalService)
    {
        _redisCache = redisCache;
        _externalService = externalService;
        _cacheExpiryTime = Convert.ToInt32(Environment.GetEnvironmentVariable("CacheExpiryTime"));
    }

    /// <summary>
    /// Invalidates the cache by updating it with new data.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult.</returns>
    [HttpPost]
    [Route("invalidate")]
    public async Task<IActionResult> InvalidateCache()
    {
        // There are two ways to invalidate the cache:
        // 1. Directly delete the cache for a specific key.
        // 2. Update the cache with new data.

        // The second approach is chosen to update the cache with new data because:
        // If the API takes 10 seconds to get the data and the cache is deleted,
        // then the user will have to wait for 10 seconds to get the data.
        // So, the cache is updated with new data.

        // Fetch new data from the external service.
        var data = await _externalService.GetDataAsync();

        await _redisCache.SetCacheValueAsync(
            AppConstants.RedisCacheKey,
            JsonConvert.SerializeObject(data),
            TimeSpan.FromSeconds(_cacheExpiryTime));

        return NoContent();
    }
}
