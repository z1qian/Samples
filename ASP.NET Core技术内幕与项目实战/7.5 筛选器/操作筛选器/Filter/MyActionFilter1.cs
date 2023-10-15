using Microsoft.AspNetCore.Mvc.Filters;

namespace 操作筛选器.Filter;

public class MyActionFilter1 : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Console.WriteLine("MyActionFilter 1:开始执行");

        /*
         * next来执行下一个操作筛选器，如果这是最后一个操作筛选器，它就会执行实际的操作方法。
         * next之前的代码是在操作方法执行之前要执行的代码，而next之后的代码则是在操作方法执行之后要执行的代码。
         */
        ActionExecutedContext r = await next();

        if (r.Exception != null)
            Console.WriteLine("MyActionFilter 1:执行失败");
        else
            Console.WriteLine("MyActionFilter 1:执行成功");
    }
}