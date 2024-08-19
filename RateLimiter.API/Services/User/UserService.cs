using Newtonsoft.Json;
using RateLimiter.API.Mock;
using RateLimiter.API.Model;
using RateLimiter.API.Services.Redis;

namespace RateLimiter.API.Services;

public class UserService : IUserService
{
    private readonly IRedisCacheService _redisCacheService;
    private readonly IExternalService _externalService;
    private readonly int _cacheExpiryTime;
    private readonly string _cacheKey;

    public UserService(
        IRedisCacheService redisCacheService,
        IExternalService externalService)
    {
        _externalService = externalService;
        _redisCacheService = redisCacheService;
        _cacheExpiryTime = Convert.ToInt32(Environment.GetEnvironmentVariable("CacheExpiryTime"));
        _cacheKey = "externalApiData";
    }
    public async Task<List<User>> GetAllAsync()
    {

        var cachedData = await _redisCacheService.GetCacheValueAsync(_cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonConvert.DeserializeObject<List<User>>(cachedData);
        }

        var data = await _externalService.GetDataAsync();

        await _redisCacheService.
            SetCacheValueAsync(
                _cacheKey,
                JsonConvert.SerializeObject(data),
                TimeSpan.FromSeconds(_cacheExpiryTime));

        return data;
    }
}
