using Senparc.Ncf.Repository;
using Senparc.Xncf.Accounts.Domain.Models;
using System;

namespace Senparc.Xncf.Accounts.Domain.Services;

public class AccountOperationLogService/* : BaseClientService<AccountOperationLog>*/
{
    //public AccountOperationLogService(IClientRepositoryBase<AccountOperationLog> repo, IServiceProvider serviceProvider) : base(repo, serviceProvider)
    //{
    //}

    public AccountOperationLog Create(string note, string @operator, DateTime operateTime)
    {
        var accountOperationLog = new AccountOperationLog(note, @operator, operateTime);

        return accountOperationLog;
    }
}