using Microsoft.Extensions.Logging;

namespace NLog的使用;

internal class Runner
{
    private readonly ILogger<Runner> _logger;

    public Runner(ILogger<Runner> logger)
    {
        _logger = logger;
    }

    public void DoAction()
    {
        _logger.LogDebug("开始执行同步数据库");
        _logger.LogDebug("连接数据库成功");

        _logger.LogWarning("查找数据失败，重试第一次");
        _logger.LogWarning("查找数据失败，重试第二次");

        _logger.LogError("查找数据最终失败");
    }
}
