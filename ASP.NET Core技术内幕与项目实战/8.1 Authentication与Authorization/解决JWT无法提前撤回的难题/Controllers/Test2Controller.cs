using Microsoft.AspNetCore.Mvc;

namespace 解决JWT无法提前撤回的难题.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]

public class Test2Controller : ControllerBase
{
    [HttpGet]
    public IActionResult Hello()
    {
        return Ok("Hello");
    }
}
