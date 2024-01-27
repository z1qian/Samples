using Senparc.Weixin.AspNet;
using Senparc.Weixin.MP;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.TenPay;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//使用本地缓存必须添加
builder.Services.AddMemoryCache();

#region 添加微信配置（一行代码）

//Senparc.Weixin 注册（必须）
builder.Services.AddSenparcWeixinServices(builder.Configuration);

#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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
        register
        .RegisterMpAccount(weixinSetting, "【我的测试公众号】公众号")
        .RegisterMpAccount(weixinSetting.Items["第二个公众号"], "【SDKWeixin】公众号")
        .RegisterTenpayV3(weixinSetting.Items["第二个公众号"], "【SDKWeixin】公众号");
    });
#endregion

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
