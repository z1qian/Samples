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

//ʹ�ñ��ػ���������
builder.Services.AddMemoryCache();

#region ���΢�����ã�һ�д��룩

//Senparc.Weixin ע�ᣨ���룩
builder.Services.AddSenparcWeixinServices(builder.Configuration);

#endregion

var app = builder.Build();

#region ����΢�����ã�һ����룩

//�ֶ���ȡ������Ϣ��ʹ�����·���
//var senparcWeixinSetting = app.Services.GetService<IOptions<SenparcWeixinSetting>>()!.Value;

//����΢�����ã����룩
var registerService = app.UseSenparcWeixin(app.Environment,
    null /* ��Ϊ null �򸲸� appsettings  �е� SenpacSetting ����*/,
    null /* ��Ϊ null �򸲸� appsettings  �е� SenpacWeixinSetting ����*/,
    register => { /* CO2NET ȫ������ */ },
    (register, weixinSetting) =>
    {
        //ע�ṫ�ں���Ϣ������ִ�ж�Σ�ע�������ںţ�
        register.RegisterMpAccount(weixinSetting, "���ҵĲ��Թ��ںš����ں�");
    });
#endregion

app.UseRouting();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

stopwatch.Stop();
TimeSpan elapsedTime = stopwatch.Elapsed;

WeixinTraceConfig();

WeixinTrace.SendCustomLog("ϵͳ��־", "ϵͳ������������ʱ�䣺" + elapsedTime.TotalSeconds + "��");
app.Run();

void WeixinTraceConfig()
{
    Senparc.CO2NET.Config.IsDebug = true;

    Senparc.Weixin.WeixinTrace.OnLogFunc = () =>
    {
        Console.WriteLine("--------------------��־�������");
    };

    Senparc.Weixin.WeixinTrace.OnWeixinExceptionFunc = ex =>
    {
        Console.WriteLine(ex.ToJson());
    };
}