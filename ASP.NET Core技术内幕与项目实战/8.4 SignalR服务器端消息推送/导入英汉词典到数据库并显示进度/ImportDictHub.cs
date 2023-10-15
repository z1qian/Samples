using Microsoft.AspNetCore.SignalR;

namespace 导入英汉词典到数据库并显示进度;

public class ImportDictHub : Hub
{
    private readonly ImportExecutor _executor;
    public ImportDictHub(ImportExecutor executor)
    {
        _executor = executor;
    }
    public Task Import()
    {
        _ = _executor.ExecuteAsync(this.Context.ConnectionId);
        return Task.CompletedTask;
    }
}