using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace 分布式缓存.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Test1Controller : ControllerBase
{
    private readonly IDistributedCache distCache;
    private readonly ILogger<Test1Controller> logger;
    public Test1Controller(IDistributedCache distCache, ILogger<Test1Controller> logger)
    {
        this.distCache = distCache;
        this.logger = logger;
    }

    [HttpGet]
    public string Now()
    {
        string? s = distCache.GetString("Now");

        if (s == null)
        {
            logger.LogInformation("往redis中写入数据");

            s = DateTime.Now.ToString();

            var opt = new DistributedCacheEntryOptions();
            opt.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);

            distCache.SetString("Now", s, opt);
        }

        logger.LogInformation("获取数据成功");
        return s;
    }
}