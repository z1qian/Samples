using Senparc.Weixin.AspNet;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.WxOpen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
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
    register => { },
    (register, weixinSetting) =>
    {
        //ע�ṫ�ں���Ϣ������ִ�ж�Σ�ע����С����
        register.RegisterWxOpenAccount(weixinSetting, "����干���С����С����");
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

app.Run();
