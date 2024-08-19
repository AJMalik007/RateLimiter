using Microsoft.AspNetCore.Mvc;
using RateLimiter.API.Services;

namespace RateLimiter.API.Controllers;
[ApiController]
[Route("api")]
public class DataController : ControllerBase
{
    private readonly ILogger<DataController> _logger;
    private readonly IUserService _userService;
    public DataController(
        ILogger<DataController> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    [Route("data")]
    public async Task<IActionResult> Get()
    {
        return Ok(await _userService.GetAllAsync());
    }
}
