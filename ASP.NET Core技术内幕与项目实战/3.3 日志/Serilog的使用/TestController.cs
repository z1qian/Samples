using Microsoft.Extensions.Logging;

namespace Serilog的使用;

internal class TestController
{
    private readonly ILogger<TestController> _logger;
    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    public void Test()
    {
        User user = new(3, "zack");

        _logger.LogWarning("新增用户{@user}", user);
        _logger.LogWarning("新增用户，Id：" + user.Id + "，Name：" + user.Name);
        _logger.LogDebug("消息发送成功了！");
    }
}

record User(int Id, string Name);
