using Microsoft.Extensions.DependencyInjection;
using Senparc.Xncf.Accounts.Domain.Services;

namespace Senparc.Xncf.AccountsTests;

public class TestBase
{
    internal IServiceCollection ServiceCollection { get; set; }
    internal IServiceProvider ServiceProvider { get; set; }

    public TestBase()
    {
        //引用或者初始化代码
        ServiceCollection = new ServiceCollection();

        ServiceCollection.AddScoped<AccountOperationLogService>();

        ServiceProvider = ServiceCollection.BuildServiceProvider();
    }
}
