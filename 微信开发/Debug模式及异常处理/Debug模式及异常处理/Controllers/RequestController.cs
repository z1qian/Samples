﻿using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.Containers;

namespace Debug模式及异常处理.Controllers;
[Route("[controller]/[action]")]
public class RequestController : Controller
{
    public async Task<ActionResult> GetAccessToken()
    {
        var token = await AccessTokenContainer.GetAccessTokenAsync(Config.SenparcWeixinSetting.WeixinAppId, true);
        Senparc.Weixin.WeixinTrace.SendCustomLog("接口日志", "获取了新的AccessToken" + token);

        return Content("最新的token：" + token);
    }

    public ActionResult DebugOpen()
    {
        Senparc.CO2NET.Config.IsDebug = true;
        Senparc.Weixin.WeixinTrace.SendCustomLog("接口日志", "Debug状态已打开。");
        return Content("Debug状态已打开。");
    }

    public ActionResult DebugClose()
    {
        Senparc.CO2NET.Config.IsDebug = false;
        Senparc.Weixin.WeixinTrace.SendCustomLog("接口日志", "Debug状态已关闭。");
        return Content("Debug状态已关闭。");
    }

    public ActionResult ThrowException()
    {
        throw new WeixinNullReferenceException($"这是一个测试{nameof(WeixinNullReferenceException)}异常");
    }
}
