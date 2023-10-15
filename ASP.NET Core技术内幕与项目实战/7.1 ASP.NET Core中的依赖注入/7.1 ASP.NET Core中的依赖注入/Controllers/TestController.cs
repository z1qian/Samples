using Microsoft.AspNetCore.Mvc;

namespace _7._1_ASP.NET_Core中的依赖注入.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly MyService1 _service1;
    public TestController(MyService1 service1)
    {
        _service1 = service1;
    }

    //[HttpGet]
    //public string Test()
    //{
    //    var names = _service1.GetNames();
    //    return string.Join(",", names);
    //}

    //大部分服务仍然通过控制器的构造方法来注入，只有使用频率不高并且比较消耗资源的服务才通过Action的参数来注入
    [HttpGet]
    public string Test([FromServices] MyService1 myService1, string name)
    {
        var names = myService1.GetNames();
        return string.Join(",", names) + ",hello:" + name;
    }
}
