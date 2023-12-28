using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

namespace 模板消息.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [HttpGet]
    public async Task<string> Send(string? openId = "o1iZy6bf5L3u_wPs8ixdGR-xoPBU")
    {
        var bag = Config.GetTemplateMessageBag(Config.AppId, "测试模板消息");
        if (bag == null)
        {
            throw new WeixinException("模板名称不存在！");
        }

        //	{{first.DATA}} 标题：{{title.DATA}} 内容：{{content.DATA}} 时间：{{time.DATA}} {{remark.DATA}}
        var data = new
        {
            //自定义颜色已去除
            first = new TemplateDataItem("模板消息测试", "#FFFF00"),
            title = new TemplateDataItem("在干嘛？", "#FFFAF0"),
            content = new TemplateDataItem("想你了你在干嘛？为什么不回我消息，请点击", "#082E54"),
            time = new TemplateDataItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            remark = new TemplateDataItem("我是备注")
        };

        string url = "http://love.qqtv5.com/web.php?id=iYPhqYm";

        var result = await Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessageAsync(null, openId, bag.TemplateId, url, data);

        return result.ToJson();
    }

    [HttpGet]
    public async Task<string> SendMessageTemplate(string? openId = "o1iZy6bf5L3u_wPs8ixdGR-xoPBU")
    {
        //	应用名称：{{keyword1.DATA}} 异常：{{keyword2.DATA}} 类型：{{keyword3.DATA}} 错误码：{{keyword4.DATA}}

        TemplateMessageExceptionNotice template = new TemplateMessageExceptionNotice("线上商城",
            "空引用", "ValuesController", "9999", "http://www.baidu.com");

        var result = await Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessageAsync(null, openId, template);

        return result.ToJson();
    }
}
