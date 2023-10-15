using Dynamic.Json;
namespace _7._6_中间件;

public class CheckAndParsingMiddleware
{
    private readonly RequestDelegate next;
    public CheckAndParsingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        string pwd = context.Request.Query["password"];
        if (pwd == "123")
        {
            if (context.Request.HasJsonContentType())
            {
                var reqStream = context.Request.BodyReader.AsStream();
                dynamic? jsonObj = DJson.Parse(reqStream);
                context.Items["BodyJson"] = jsonObj;
            }
            await next(context);
        }
        else
        {
            //请求不会被传递到其他中间件，因此也不会执行Run中的代码，这个请求就被终止了
            context.Response.StatusCode = 401;
        }
    }
}