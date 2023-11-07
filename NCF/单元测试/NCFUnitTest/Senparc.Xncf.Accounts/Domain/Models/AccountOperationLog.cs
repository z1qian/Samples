using System;

namespace Senparc.Xncf.Accounts;

public class AccountOperationLog
{
    private AccountOperationLog() { }

    public AccountOperationLog(string note, string @operator, DateTime operateTime) : this()
    {
        Note = note;
        Operator = @operator;
        OperateTime = operateTime;
    }

    public string Note { get; private set; }
    public string Operator { get; private set; }
    public DateTime OperateTime { get; private set; }

    public void ReBuildOperationType(AccountOperationType typeKind)
    {
        Note = $"[{typeKind.ToString()}]{Note}";
    }
}