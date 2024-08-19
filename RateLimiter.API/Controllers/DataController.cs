using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RateLimiter.API.Mock;
using RateLimiter.API.Model;
using RateLimiter.API.Services.Redis;

namespace RateLimiter.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private readonly ILogger<DataController> _logger;
    private readonly IExternalService _service;
    private readonly IRedisCacheService _redisCacheService;
    public DataController(
        ILogger<DataController> logger,
        IExternalService service,
        IRedisCacheService redisCacheService)
    {
        _logger = logger;
        _service = service;
        _redisCacheService = redisCacheService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        const string cacheKey = "externalApiData";
        var cachedData = await _redisCacheService.GetCacheValueAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return Ok(new
            {
                source = "cache",
                data = JsonConvert.DeserializeObject<List<User>>(cachedData)
            });
        }

        var data = await _service.GetDataAsync();

        await _redisCacheService.
            SetCacheValueAsync(
                cacheKey,
                JsonConvert.SerializeObject(data),
                TimeSpan.FromSeconds(Convert.ToInt32(Environment.GetEnvironmentVariable("CacheExpiryTime"))));

        return Ok(new { source = "api", data });
    }
}
