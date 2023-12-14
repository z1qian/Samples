using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.Entities;

namespace 接口调用及数据请求.Controllers;
public class BaseController : Controller
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    public BaseController(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    protected string AppId
    {
        get
        {
            return Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        }
    }

    protected static ISenparcWeixinSettingForMP MpSetting
    {
        get
        {
            return Config.SenparcWeixinSetting.MpSetting;
        }
    }

    protected string AppDataPath
    {
        get
        {
            // 获取应用程序根目录路径
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            // 构建 App_Data 文件夹的完整路径
            string appDataPath = Path.Combine(contentRootPath, "App_Data");

            return appDataPath;
        }
    }
}