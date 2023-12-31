﻿using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin.MP.Containers;
using System.Diagnostics;
using 接口调用凭证及数据容器_Container_.Models;

namespace 接口调用凭证及数据容器_Container_.Controllers;
public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<ActionResult> CustomMessage(string openId = "o1iZy6bf5L3u_wPs8ixdGR-xoPBU")
    {
        for (int i = 0; i < 3; i++)
        {
            await Task.Delay(500);

            try
            {
                await Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendTextAsync(AppId, openId, "从服务器发来的客服消息：" + (3 - i));
            }
            catch (Exception)
            {
                //额外的一些处理
            }
        }

        var result
            = await Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendTextAsync(AppId, openId, "客服消息发送完成！" + DateTime.Now);

        return Content(result.ToJson());
    }

    public async Task<ActionResult> CustomMessage2(string openId = "o1iZy6bf5L3u_wPs8ixdGR-xoPBU")
    {
        var result = await Task.Factory.StartNew(async () =>
             {
                 for (int i = 0; i < 3; i++)
                 {
                     try
                     {
                         await Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendTextAsync(AppId, openId, $"从服务器发来的客服消息。时间：{DateTime.Now:HH:mm:ss.ffff}，编号：{i + 1}");
                     }
                     catch (Exception ex)
                     {
                         //额外的一些处理
                         Console.WriteLine(ex.Message);
                     }
                 }

                 return Content("消息发送完毕！");
             });

        return await result;
    }

    public async Task<ActionResult> ChangeAccessToken(string openId = "o1iZy6bf5L3u_wPs8ixdGR-xoPBU")
    {
        for (int i = 0; i < 3; i++)
        {
            await Task.Delay(500);

            try
            {
                var accessToken = await AccessTokenContainer.GetAccessTokenAsync(AppId);

                await Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendTextAsync(AppId, openId, "从服务器发来的客服消息：" + (3 - i) + "，AccessToken：" + accessToken);
            }
            catch (Exception)
            {
                //额外的一些处理
            }
        }

        return Content("ok");
    }
}
