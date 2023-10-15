using _7._6_�м��;

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();
////Map���������������
//app.Map("/test", async appbuilder =>
//{
//    //���������ܵ�������ɹܵ��Ķ��Use������������Ԥ������������ɺ��ɨβ��λ
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
//    //Run������Ҫ��ҵ�����
//    appbuilder.Run(async ctx =>
//    {
//        await ctx.Response.WriteAsync("hello middleware <br/>");
//    });
//});
//app.Run();

//�����м����Ĵ���
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Map("/test", async appbuilder =>
{
    //�����м���ൽ�ܵ���
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

//������һ�����С�����ͷ��AAA��ֵΪ123���������Ӧ�Ĺܵ�
app.MapWhen(ctx => ctx.Request.Headers["AAA"] == "123", async appbuilder =>
{
    appbuilder.Run(async ctx =>
    {
        await ctx.Response.WriteAsync("AAA=123");
    });
});
//������һ����������·���ԡ�/api����ͷ�������Ӧ�Ĺܵ�
app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/api"), async appbuilder =>
{
    appbuilder.Run(async ctx =>
    {
        await ctx.Response.WriteAsync("/api");
    });
});
app.Run();