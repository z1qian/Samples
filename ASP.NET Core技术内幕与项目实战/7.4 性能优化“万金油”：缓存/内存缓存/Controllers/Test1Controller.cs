using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace 内存缓存.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class Test1Controller : ControllerBase
{
    private readonly ILogger<Test1Controller> logger;
    private readonly IMemoryCache memCache;
    public Test1Controller(IMemoryCache memCache, ILogger<Test1Controller> logger)
    {
        this.memCache = memCache;
        this.logger = logger;
    }

    //缓存项不会过期
    [HttpGet]
    public async Task<Book[]> GetBooks()
    {
        logger.LogInformation("开始执行GetBooks");
        var items = await memCache.GetOrCreateAsync("AllBooks", async (e) =>
        {
            logger.LogInformation("从数据库中读取数据");
            return new Book[]
            {
                new Book(1,"图书1",30),
                new Book(2,"图书2",40.3),
                new Book(3,"图书3",50.1),
            };
        });
        logger.LogInformation("把数据返回给调用者");
        return items;
    }

    //设置绝对过期时间
    [HttpGet]
    public async Task<Book[]> Demo1()
    {
        logger.LogInformation("开始执行Demo1：" + DateTime.Now);
        var items = await memCache.GetOrCreateAsync("AllBooks", async (e) =>
        {
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
            e.RegisterPostEvictionCallback((key, value, r, s) =>
                logger.LogInformation($"缓存key:{key}已失效！,value:{value}"));

            logger.LogInformation("从数据库中读取数据");
            return new Book[]
            {
              new Book(1,"图书1",30),
              new Book(2,"图书2",40.3),
              new Book(3,"图书3",50.1),
            };
        });
        logger.LogInformation("Demo1执行结束");
        return items;
    }

    //设置滑动过期时间
    [HttpGet]
    public async Task<Book[]> Demo2()
    {
        logger.LogInformation("开始执行Demo2：" + DateTime.Now);
        var items = await memCache.GetOrCreateAsync("AllBooks2", async (e) =>
        {
            e.SlidingExpiration = TimeSpan.FromSeconds(10);
            e.RegisterPostEvictionCallback((key, value, r, s) =>
                logger.LogInformation($"缓存key:{key}已失效！,value:{value}"));

            logger.LogInformation("从数据库中读取数据");
            return new Book[]
            {
              new Book(1,"图书1",30),
              new Book(2,"图书2",40.3),
              new Book(3,"图书3",50.1),
            };
        });
        logger.LogInformation("Demo2执行结束");
        return items;
    }

    //混合使用过期时间策略
    [HttpGet]
    public async Task<Book[]> Demo3()
    {
        logger.LogInformation("开始执行Demo3：" + DateTime.Now);
        var items = await memCache.GetOrCreateAsync("AllBooks3", async (e) =>
        {
            e.SlidingExpiration = TimeSpan.FromSeconds(10);
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);

            logger.LogInformation("从数据库中读取数据");
            return new Book[]
            {
              new Book(1,"图书1",30),
              new Book(2,"图书2",40.3),
              new Book(3,"图书3",50.1),
            };
        });
        logger.LogInformation("Demo3执行结束");
        return items;
    }

    [HttpGet]
    //不适当的缓存代码
    public async Task<ActionResult<Book?>> Demo5(int id)
    {
        string cacheKey = "Book" + id;             //缓存键
        Book? b = memCache.Get<Book?>(cacheKey);

        //如果b一直为null的话，一直会查数据库，存在缓存穿透问题
        if (b == null)                                //如果缓存中没有数据
        {
            //查询数据库，然后写入缓存
            //b = new Book(1, "图书1", 30);
            b = null;
            memCache.Set(cacheKey, b);
        }
        if (b == null)                                //如果仍然没找到数据
            return NotFound("找不到这本书");
        else
            return b;
    }

    [HttpGet]
    //使用GetOrCreateAsync方法规避缓存穿透
    public async Task<ActionResult<Book?>> Demo6(int id)
    {
        logger.LogInformation("开始执行Demo6");
        string cacheKey = "Book" + id;
        //GetOrCreateAsync这个方法会把null也当成合法的缓存值，这样就可以轻松规避缓存穿透的问题了
        var book = await memCache.GetOrCreateAsync(cacheKey, async (e) =>
        {
            Book? b = null;
            logger.LogInformation("数据库查询：{0}", b == null ? "为空" : "不为空");
            return b;
        });
        logger.LogInformation("Demo6执行结束:{0}", book == null ? "为空" : "不为空");
        return book;
    }

    ////有数据泄露问题的代码
    //public User GetUserInfo()
    //{
    //    Guid userId = ...;//获取当前用户ID
    //    return memCache.GetOrCreate("UserInfo", (e) =>    //用UserInfo+userId作为缓存键
    //    {
    //        return ctx.User.Find(userId);
    //    });
    //}
}