using Microsoft.Extensions.Logging;

namespace SystemServices;

internal class Runner2
{
    private readonly ILogger<Runner2> _logger;

    public Runner2(ILogger<Runner2> logger)
    {
        _logger = logger;
    }

    public void DoAction()
    {
        _logger.LogDebug("开始执行同步FTP");
        _logger.LogDebug("连接FTP成功");

        _logger.LogWarning("查找数据失败，重试第一次");
        _logger.LogWarning("查找数据失败，重试第二次");

        _logger.LogError("查找数据最终失败");
    }
}
