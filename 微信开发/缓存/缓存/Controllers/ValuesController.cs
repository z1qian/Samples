using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Cache;
using Senparc.Weixin.Cache;

namespace 缓存.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private static int count = 0;
    public static int Count
    {
        get
        {
            Thread.Sleep(100);
            return count;
        }
        set
        {
            Thread.Sleep(100);
            count = value;
        }
    }

    [HttpGet]
    public int GetCount()
    {
        //var stragety = ContainerCacheStrategyFactory.GetContainerCacheStrategyInstance().BaseCacheStrategy();

        //using (stragety.BeginCacheLock("SenparcClass", "LockTest"))
        var strategy = CacheStrategyFactory.GetObjectCacheStrategyInstance();
        using (strategy.BeginCacheLock("SenparcClass", "LockTest"))
        {
            var count = Count;
            Thread.Sleep(2);
            Count = count + 1;

            return Count;
        }
    }

    private int totalThreadCount = 100;
    private int finishedThreadCount = 0;
    [HttpPost]
    public void LockTest()
    {
        List<Thread> threads = new List<Thread>();
        for (int i = 0; i < totalThreadCount; i++)
        {
            var thread = new Thread(RunSingleRequest);
            thread.Start();
        }

        //while (finishedThreadCount <= totalThreadCount)
        //{

        //}

        //Console.WriteLine("测试完成，线程总数：" + totalThreadCount);
    }

    private void RunSingleRequest()
    {
        Console.WriteLine("结果：" + GetCount());

        finishedThreadCount++;
    }
}
