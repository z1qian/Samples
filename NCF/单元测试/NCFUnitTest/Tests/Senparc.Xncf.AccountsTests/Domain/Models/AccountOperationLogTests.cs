using Senparc.Xncf.Accounts.Domain.Models;

namespace Senparc.Xncf.AccountsTests.Domain.Models;

[TestClass]
public class AccountOperationLogTests
{
    [TestMethod]
    public void CreateTest()
    {
        var note = "这里是备注信息";
        var @operator = "操作人";
        var operateTime = SystemTime.Now.DateTime;

        var accountOperationLog = new AccountOperationLog(note, @operator, operateTime);

        Assert.AreEqual(note, accountOperationLog.Note);
        Assert.AreEqual(@operator, accountOperationLog.Operator);
        Assert.AreEqual(operateTime, accountOperationLog.OperateTime);
    }

    [TestMethod]
    public void ReBuildNoteTest()
    {
        var typeKind = AccountOperationType.Message;
        var note = "这里是备注信息";
        var @operator = "操作人";
        var operateTime = SystemTime.Now.DateTime;

        var accountOperationLog = new AccountOperationLog(note, @operator, operateTime);

        accountOperationLog.ReBuildOperationType(typeKind);

        Assert.AreEqual("[Message]" + note, accountOperationLog.Note);
    }

}
