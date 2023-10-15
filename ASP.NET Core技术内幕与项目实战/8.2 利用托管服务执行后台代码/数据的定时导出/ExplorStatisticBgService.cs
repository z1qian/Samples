using System.Text;

namespace 数据的定时导出;

public class ExplortStatisticBgService : BackgroundService
{
    private readonly TestDbContext _ctx;
    private readonly ILogger<ExplortStatisticBgService> _logger;
    private readonly IServiceScope _serviceScope;
    public ExplortStatisticBgService(IServiceScopeFactory scopeFactory)
    {
        _serviceScope = scopeFactory.CreateScope();
        var sp = _serviceScope.ServiceProvider;
        _ctx = sp.GetRequiredService<TestDbContext>();
        _logger = sp.GetRequiredService<ILogger<ExplortStatisticBgService>>();
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("开始执行统计");
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("统计进行中" + DateTime.Now);
            try
            {
                await DoExecuteAsync();
                await Task.Delay(5000);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户统计数据失败");
                await Task.Delay(1000);
            }
        }
    }
    private async Task DoExecuteAsync()
    {
        var items = _ctx.Users
            .GroupBy(u => u.CreationTime.Date)
            .Select(e => new { Date = e.Key, Count = e.Count() });

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Date:{DateTime.Now}");

        foreach (var item in items)
        {
            sb.Append(item.Date).AppendLine($":{item.Count}");
        }
        await File.WriteAllTextAsync("1.txt", sb.ToString());
        _logger.LogInformation($"导出完成");
    }
    public override void Dispose()
    {
        base.Dispose();
        _serviceScope.Dispose();
    }
}
