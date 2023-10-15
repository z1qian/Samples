using Microsoft.AspNetCore.Mvc;

namespace _7._4_性能优化_万金油__缓存.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Test1Controller : ControllerBase
{
    [HttpGet]
    //客户端响应缓存
    [ResponseCache(Duration = 60)]
    public DateTime Now()
    {
        return DateTime.Now;
    }
}
