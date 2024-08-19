using Microsoft.AspNetCore.Mvc;
using RateLimiter.API.Services;

namespace RateLimiter.API.Controllers;

[ApiController]
[Route("api")]
public class DataController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataController"/> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    public DataController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Gets all user data.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing the user data.</returns>
    [HttpGet]
    [Route("data")]
    public async Task<IActionResult> Get()
    {
        return Ok(await _userService.GetAllAsync());
    }
}
