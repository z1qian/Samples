using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Transactions;

namespace 自动启用事务的操作筛选器;

public class TransactionScopeFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        bool hasNotTransactionalAttribute = false;
        if (context.ActionDescriptor is ControllerActionDescriptor)
        {
            var actionDesc = (ControllerActionDescriptor)context.ActionDescriptor;
            hasNotTransactionalAttribute = actionDesc.MethodInfo.IsDefined(typeof(NotTransactionalAttribute));
        }
        if (hasNotTransactionalAttribute)
        {
            await next();
            return;
        }
        //当一段使用EF Core进行数据库操作的代码放到TransactionScope声明的范围中的时候，这段代码就会自动被标记为“支持事务”。
        //TransactionScope实现了IDisposable接口，如果一个TransactionScope的对象没有调用Complete就执行了Dispose方法，则事务会被回滚，否则事务就会被提交。
        using var txScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var result = await next();
        if (result.Exception == null)//操作方法执行没有异常
        {
            txScope.Complete();
        }
    }
}
