using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Senparc.Weixin;
using Senparc.Weixin.MP.Helpers;

namespace JS_SDK.Controllers;
public class JSSDKController : Controller
{
    public string? UserName { get; set; }

    public IActionResult Index()
    {
        //var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(Config.SenparcWeixinSetting.WeixinAppId,
        //    Config.SenparcWeixinSetting.WeixinAppSecret, HttpContext.Request.GetDisplayUrl());

        return View();
    }

    public IActionResult OtherView()
    {
        //var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(Config.SenparcWeixinSetting.WeixinAppId,
        //  Config.SenparcWeixinSetting.WeixinAppSecret, HttpContext.Request.GetDisplayUrl());

        return View();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        UserName = "子骞";
    }


    public override void OnActionExecuted(ActionExecutedContext context)
    {
        base.OnActionExecuted(context);

        UserName ??= "空";
        ViewData["UserName"] = UserName;

        var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(Config.SenparcWeixinSetting.WeixinAppId,
          Config.SenparcWeixinSetting.WeixinAppSecret, HttpContext.Request.GetDisplayUrl());
        ViewData.Model = jssdkUiPackage;
    }
}
