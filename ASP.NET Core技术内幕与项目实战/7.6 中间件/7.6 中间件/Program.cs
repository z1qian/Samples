using _7._6_中间件;

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();
////Map是用来引入请求的
//app.Map("/test", async appbuilder =>
//{
//    //请求来到管道后，由组成管道的多个Use负责对请求进行预处理及请求处理完成后的扫尾工位
//    appbuilder.Use(async (context, next) =>
//    {
//        context.Response.ContentType = "text/html";
//        await context.Response.WriteAsync("1  Start<br/>");
//        await next.Invoke();
//        await context.Response.WriteAsync("1  End<br/>");
//    });
//    appbuilder.Use(async (context, next) =>
//    {
//        await context.Response.WriteAsync("2  Start<br/>");
//        await next.Invoke();
//        await context.Response.WriteAsync("2  End<br/>");
//    });
//    //Run负责主要的业务规则
//    appbuilder.Run(async ctx =>
//    {
//        await ctx.Response.WriteAsync("hello middleware <br/>");
//    });
//});
//app.Run();

//调用中间件类的代码
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Map("/test", async appbuilder =>
{
    //引入中间件类到管道中
    appbuilder.UseMiddleware<CheckAndParsingMiddleware>();
    appbuilder.Run(async ctx =>
    {
        ctx.Response.ContentType = "text/html";
        ctx.Response.StatusCode = 200;
        dynamic? jsonObj = ctx.Items["BodyJson"];
        int i = jsonObj.i;
        int j = jsonObj.j;
        await ctx.Response.WriteAsync($"{i}+{j}={i + j}");
    });
});

//定义了一个所有“报文头中AAA的值为123”的请求对应的管道
app.MapWhen(ctx => ctx.Request.Headers["AAA"] == "123", async appbuilder =>
{
    appbuilder.Run(async ctx =>
    {
        await ctx.Response.WriteAsync("AAA=123");
    });
});
//定义了一个所有请求路径以“/api”开头的请求对应的管道
app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/api"), async appbuilder =>
{
    appbuilder.Run(async ctx =>
    {
        await ctx.Response.WriteAsync("/api");
    });
});
app.Run();