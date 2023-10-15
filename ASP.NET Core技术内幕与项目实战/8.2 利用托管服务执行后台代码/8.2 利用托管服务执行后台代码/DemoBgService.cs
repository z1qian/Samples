namespace _8._2_利用托管服务执行后台代码;

/// <summary>
/// 托管服务会随着应用程序启动，当然，托管服务是在后台运行的，不会阻塞ASP.NET Core中其他程序的运行。
/// 
/// 托管服务是以单例的生命周期注册到依赖注入容器中的
/// </summary>
public class DemoBgService : BackgroundService
{
    private ILogger<DemoBgService> logger;
    public DemoBgService(ILogger<DemoBgService> logger)
    {
        this.logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("准备读取文件");
            await Task.Delay(5000);
            string s = await File.ReadAllTextAsync("1.txt");
            await Task.Delay(20000);
            logger.LogInformation(s);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "托管服务中发生异常");
        }
    }
}
