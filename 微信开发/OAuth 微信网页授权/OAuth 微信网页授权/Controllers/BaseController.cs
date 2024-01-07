using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.Entities;

namespace OAuth_微信网页授权.Controllers;

public class BaseController : Controller
{
    protected string AppId
    {
        get
        {
            return Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        }
    }

    protected string AppSecret
    {
        get
        {
            return Config.SenparcWeixinSetting.WeixinAppSecret;
        }
    }

    protected static ISenparcWeixinSettingForMP MpSetting
    {
        get
        {
            return Config.SenparcWeixinSetting.MpSetting;
        }
    }
}
