using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace 用MediatR实现领域事件.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Test1Controller : ControllerBase
{
    //在需要发布事件的类中注入IMediator类型的服务
    private readonly IMediator _mediator;
    public Test1Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromQuery] string userName)
    {
        //然后我们调用Publish方法来发布
        //注意不要错误地调用Send方法来发布事件，因为Send方法是用来发布一对一事件的，而Publish方法是用来发布一对多事件的
        //会等待所有的事件处理者执行完毕，代码再向下执行
        await _mediator.Publish(new TestEvent(userName));

        return Ok("ok");
    }
}