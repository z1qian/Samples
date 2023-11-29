using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Cache;
using System.Diagnostics;

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
            return count;
        }
        set
        {
            Thread.Sleep(100);
            count = value;
        }
    }

    [HttpPost]
    public void GetCount()
    {
        //var stragety = ContainerCacheStrategyFactory.GetContainerCacheStrategyInstance().BaseCacheStrategy();
        //using (stragety.BeginCacheLock("SenparcClass", "LockTest"))

        var strategy = CacheStrategyFactory.GetObjectCacheStrategyInstance();
        using (strategy.BeginCacheLock("SenparcClass", "LockTest"))
        {
            int c = Count;

            Count = c + 1;
            Debug.WriteLine("结果：" + Count);

            finishedThreadCount++;
        }
    }

    private const int totalThreadCount = 100;
    private int finishedThreadCount = 0;
    [HttpPost]
    public void LockTest()
    {
        List<Thread> threads = new List<Thread>();
        for (int i = 0; i < totalThreadCount; i++)
        {
            var thread = new Thread(GetCount);
            thread.Start();
        }

        while (finishedThreadCount < totalThreadCount)
        {
        }

        Debug.WriteLine("测试完成，线程总数：" + finishedThreadCount);
    }
}
