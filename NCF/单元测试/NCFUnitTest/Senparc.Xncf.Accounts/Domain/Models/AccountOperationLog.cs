﻿using Senparc.Ncf.Core.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Senparc.Xncf.Accounts.Domain.Models;

[Serializable]
[Table("AccountOperationLogs")]
public class AccountOperationLog : EntityBase<int>
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
        Note = $"[{typeKind}]{Note}";
    }
}