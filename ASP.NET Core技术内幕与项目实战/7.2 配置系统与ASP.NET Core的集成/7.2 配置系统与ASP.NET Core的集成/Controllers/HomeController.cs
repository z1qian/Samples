using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace _7._2_配置系统与ASP.NET_Core的集成.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly IOptionsSnapshot<SmtpOptions> smtpOptions;
    private readonly IConnectionMultiplexer connMultiplexer;
    public HomeController(IOptionsSnapshot<SmtpOptions> smtpOptions,
       IConnectionMultiplexer connMultiplexer)
    {
        this.smtpOptions = smtpOptions;
        this.connMultiplexer = connMultiplexer;
    }
    [HttpGet]
    public string Index()
    {
        var opt = smtpOptions.Value;
        var timeSpan = connMultiplexer.GetDatabase().Ping();
        return $"Smtp:{opt} timeSpan:{timeSpan}";
    }
}