using Microsoft.AspNetCore.Mvc;

namespace HRMS.API;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Working");
    }
}