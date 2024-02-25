using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.Entities;

namespace 后端数据请求.Controllers;
public class BaseController : Controller
{
    protected string WxOpenAppId
    {
        get
        {
            return Config.SenparcWeixinSetting.WxOpenAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        }
    }

    protected string WxOpenAppSecret
    {
        get
        {
            return Config.SenparcWeixinSetting.WxOpenAppSecret;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        }
    }

    protected string WxOpenToken
    {
        get
        {
            return Config.SenparcWeixinSetting.WxOpenToken;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        }
    }

    protected string WxOpenEncodingAESKey
    {
        get
        {
            return Config.SenparcWeixinSetting.WxOpenEncodingAESKey;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        }
    }

    protected static ISenparcWeixinSettingForWxOpen WxOpenSetting
    {
        get
        {
            return Config.SenparcWeixinSetting.WxOpenSetting;
        }
    }
}