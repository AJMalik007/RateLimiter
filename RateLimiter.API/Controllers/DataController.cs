using Microsoft.AspNetCore.Mvc;
using RateLimiter.API.Services;

namespace RateLimiter.API.Controllers;
[ApiController]
[Route("api")]
public class DataController : ControllerBase
{
    private readonly IUserService _userService;

    public DataController(
        IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Route("data")]
    public async Task<IActionResult> Get()
    {
        return Ok(await _userService.GetAllAsync());
    }
}
