using Microsoft.AspNetCore.Mvc;
using Zack.EventBus;

namespace 发送集成事件.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IEventBus _eventBus;

    public TestController(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    [HttpPost]
    public string Publish()
    {
        _eventBus.Publish("UserAdded", new { UserName = "yzk", Age = 18 });
        return "ok";
    }
}
