using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.Entities;

namespace 模板消息.Controllers;

public class BaseController : Controller
{
    protected string AppId
    {
        get
        {
            return Senparc.Weixin.Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        }
    }

    protected static ISenparcWeixinSettingForMP MpSetting
    {
        get
        {
            return Senparc.Weixin.Config.SenparcWeixinSetting.MpSetting;
        }
    }
}
