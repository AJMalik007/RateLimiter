using Newtonsoft.Json;
using RateLimiter.API.Common;
using RateLimiter.API.Mock;
using RateLimiter.API.Model;
using RateLimiter.API.Services.Redis;

namespace RateLimiter.API.Services;

/// <summary>
/// Service for user-related operations.
/// </summary>
public class UserService : IUserService
{
    private readonly IRedisCacheService _redisCacheService;
    private readonly IExternalService _externalService;
    private readonly int _cacheExpiryTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="redisCacheService">The Redis cache service.</param>
    /// <param name="externalService">The external service to fetch data from.</param>
    public UserService(
        IRedisCacheService redisCacheService,
        IExternalService externalService)
    {
        _externalService = externalService;
        _redisCacheService = redisCacheService;
        _cacheExpiryTime = Convert.ToInt32(Environment.GetEnvironmentVariable("CacheExpiryTime"));
    }

    /// <summary>
    /// Asynchronously retrieves all users.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of users.</returns>
    public async Task<List<User>> GetAllAsync()
    {
        // Try to get data from cache
        var cachedData = await _redisCacheService.GetCacheValueAsync(AppConstants.RedisCacheKey);

        // If cached data is available, deserialize and return it
        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonConvert.DeserializeObject<List<User>>(cachedData);
        }

        // If no cached data, fetch data from external service
        var data = await _externalService.GetDataAsync();

        // Cache the fetched data
        await _redisCacheService.SetCacheValueAsync(
            AppConstants.RedisCacheKey,
            JsonConvert.SerializeObject(data),
            TimeSpan.FromSeconds(_cacheExpiryTime));

        return data;
    }
}
