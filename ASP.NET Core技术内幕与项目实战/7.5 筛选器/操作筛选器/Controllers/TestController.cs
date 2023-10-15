using Microsoft.AspNetCore.Mvc;

namespace 操作筛选器.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet]
    public string GetData()
    {
        Console.WriteLine("执行GetData");
        return "yzk";
    }
}
