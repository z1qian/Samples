using Senparc.WebSocket;
using Senparc.Weixin.AspNet;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.WxOpen;
using 后端数据请求.WebSocket;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//使用本地缓存必须添加
builder.Services.AddMemoryCache();

#region 添加微信配置（一行代码）

//Senparc.Weixin 注册（必须）
builder.Services.AddSenparcWeixinServices(builder.Configuration)
    .AddSenparcWebSocket<CustomNetCoreWebSocketMessageHandler>(); //Senparc.WebSocket 注册（按需）

#endregion

var app = builder.Build();

#region 启用微信配置（一句代码）

//手动获取配置信息可使用以下方法
//var senparcWeixinSetting = app.Services.GetService<IOptions<SenparcWeixinSetting>>()!.Value;

//启用微信配置（必须）
var registerService = app.UseSenparcWeixin(app.Environment,
    null /* 不为 null 则覆盖 appsettings  中的 SenpacSetting 配置*/,
    null /* 不为 null 则覆盖 appsettings  中的 SenpacWeixinSetting 配置*/,
    register => { },
    (register, weixinSetting) =>
    {
        //注册公众号信息（可以执行多次，注册多个小程序）
        register.RegisterWxOpenAccount(weixinSetting, "【子骞测试小程序】小程序");
    });
#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//使用 SignalR（.NET Core 3.0）                                                      -- DPBMARK WebSocket
app.UseEndpoints(endpoints =>
{
    //配置自定义 SenparcHub
    endpoints.MapHub<SenparcHub>("/SenparcHub");
});

app.Run();
