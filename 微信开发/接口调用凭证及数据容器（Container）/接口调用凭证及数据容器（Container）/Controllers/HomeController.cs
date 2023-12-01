using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
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
}
