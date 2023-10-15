using Microsoft.AspNetCore.Mvc;

namespace _8._3_请求数据的校验.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpPost]
    public Login2Request TestMethod(Login2Request request)
    {
        return request;
    }
}
