using Microsoft.AspNetCore.Mvc;

namespace RateLimiter.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private readonly ILogger<DataController> _logger;

    public DataController(ILogger<DataController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok("OK");
    }
}
