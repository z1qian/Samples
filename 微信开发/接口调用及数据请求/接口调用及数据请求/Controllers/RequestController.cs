using Microsoft.AspNetCore.Mvc;
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

    public void Test()
    {
        Senparc.CO2NET.HttpUtility.js
    }
}
