using Senparc.CO2NET.Extensions;
using Senparc.Weixin;
using Senparc.Weixin.AspNet;
using Senparc.Weixin.MP;
using Senparc.Weixin.RegisterServices;
using System.Diagnostics;

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//使用本地缓存必须添加
builder.Services.AddMemoryCache();

#region 添加微信配置（一行代码）

//Senparc.Weixin 注册（必须）
builder.Services.AddSenparcWeixinServices(builder.Configuration);

#endregion

var app = builder.Build();

#region 启用微信配置（一句代码）

//手动获取配置信息可使用以下方法
//var senparcWeixinSetting = app.Services.GetService<IOptions<SenparcWeixinSetting>>()!.Value;

//启用微信配置（必须）
var registerService = app.UseSenparcWeixin(app.Environment,
    null /* 不为 null 则覆盖 appsettings  中的 SenpacSetting 配置*/,
    null /* 不为 null 则覆盖 appsettings  中的 SenpacWeixinSetting 配置*/,
    register => { /* CO2NET 全局配置 */ },
    (register, weixinSetting) =>
    {
        //注册公众号信息（可以执行多次，注册多个公众号）
        register.RegisterMpAccount(weixinSetting, "【我的测试公众号】公众号");
    });
#endregion

app.UseRouting();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

stopwatch.Stop();
TimeSpan elapsedTime = stopwatch.Elapsed;

WeixinTraceConfig();

WeixinTrace.SendCustomLog("系统日志", "系统已启动，启动时间：" + elapsedTime.TotalSeconds + "秒");
app.Run();

void WeixinTraceConfig()
{
    Senparc.CO2NET.Config.IsDebug = true;

    Senparc.Weixin.WeixinTrace.OnLogFunc = () =>
    {
        Console.WriteLine("--------------------日志被输出！");
    };

    Senparc.Weixin.WeixinTrace.OnWeixinExceptionFunc = ex =>
    {
        Console.WriteLine("发生微信异常：" + ex.ToJson());
    };
}