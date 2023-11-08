using Microsoft.Extensions.DependencyInjection;
using Senparc.Xncf.Accounts.Domain.Services;

namespace Senparc.Xncf.AccountsTests.Domain.Services;

[TestClass]
public class AccountOperationLogServiceTests : TestBase
{
    [TestMethod]
    public void CreateTest()
    {
        var note = "这里是备注信息";
        var @operator = "操作人";
        var operateTime = SystemTime.Now.DateTime;

        AccountOperationLogService service = base.ServiceProvider.GetRequiredService<AccountOperationLogService>();

        var accountOperationLog = service.Create(note, @operator, operateTime);

        Assert.IsNotNull(accountOperationLog, "accountOperationLog 为 null");
    }
}
