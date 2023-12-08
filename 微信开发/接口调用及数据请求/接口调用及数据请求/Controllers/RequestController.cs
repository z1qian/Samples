using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace 接口调用及数据请求.Controllers;

[Route("[controller]/[action]")]
public class RequestController : BaseController
{
    private readonly IServiceProvider _serviceProvider;

    public RequestController(IServiceProvider sp)
    {
        _serviceProvider = sp;
    }

    [HttpGet]
    public async Task<ActionResult> Get(string url = "http://www.baidu.com")
    {
        var html = await Senparc.CO2NET.HttpUtility.RequestUtility.HttpGetAsync(_serviceProvider, url, Encoding.UTF8);

        html += "<script>alert('This is a remote page')</script>";

        return Content(html, "text/html");
    }

    [HttpGet]
    public async Task<ActionResult> SimulateLogin(string url = "http://www.baidu.com")
    {
        var cookieContainer = new CookieContainer();
        var html = await Senparc.CO2NET.HttpUtility.RequestUtility.HttpGetAsync(_serviceProvider,
            url, cookieContainer, Encoding.UTF8);

        return Content(html, "text/html");
    }

    [HttpGet]
    public async Task<ActionResult> Post(string url = "https://sdk.weixin.senparc.com/AsyncMethods/TemplateMessageTest", string code = "")
    {
        //设置Fiddler代理
        //Senparc.CO2NET.HttpUtility.RequestUtility.SetHttpProxy("http://127.0.0.1", "8888", null, null);

        Dictionary<string, string> formData = new Dictionary<string, string>()
        {
            ["checkcode"] = code,
            ["__RequestVerificationToken"] = "CfDJ8HJ8aLs5PrhKoiuWe5Q_4l2C2LYTPsFmVBPS8KcgQ64OIO3zhufBfiM-VDcqfeAx17TEoj3pTT-RF9-7JnmiV592tOUVNaMSdVScTr4LBXwd8IibD5mQLi-SXzoT0gOEFWX1foFc4I5qfyUpC75a65E"
        };
        var html = await Senparc.CO2NET.HttpUtility.RequestUtility.HttpPostAsync(_serviceProvider, url,
            formData: formData, encoding: Encoding.UTF8);
        html = $"<span style='color:red'>{html}</span>";

        //清除代理
        //Senparc.CO2NET.HttpUtility.RequestUtility.RemoveHttpProxy();

        return Content(html, "text/html", Encoding.UTF8);
    }
}
