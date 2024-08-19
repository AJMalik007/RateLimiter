using Microsoft.AspNetCore.Mvc;
using RateLimiter.API.Mock;

namespace RateLimiter.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private readonly ILogger<DataController> _logger;
    private readonly IExternalService _service;

    public DataController(
        ILogger<DataController> logger,
        IExternalService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {


        return Ok(await _service.GetDataAsync());
    }
}
