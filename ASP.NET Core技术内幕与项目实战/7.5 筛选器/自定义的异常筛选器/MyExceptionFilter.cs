using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace 自定义的异常筛选器
{
    //需要注意的是，只有ASP.NET Core线程中的未处理异常才会被异常筛选器处理，后台线程中的异常不会被异常筛选器处理
    public class MyExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<MyExceptionFilter> logger;
        private readonly IHostEnvironment env;
        public MyExceptionFilter(ILogger<MyExceptionFilter> logger, IHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
        }
        public Task OnExceptionAsync(ExceptionContext context)
        {
            Exception exception = context.Exception;
            logger.LogError(exception, "UnhandledException occured");

            string message;
            if (env.IsDevelopment())
                message = exception.ToString();
            else
                message = "程序中出现未处理异常";

            ObjectResult result = new ObjectResult(new { code = 500, message = message });
            result.StatusCode = 500;

            context.Result = result;
            //我们设置context.ExceptionHandled的值为true，通过这样的方式来告知ASP.NET Core不再执行默认的异常响应逻辑
            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }
    }
}
