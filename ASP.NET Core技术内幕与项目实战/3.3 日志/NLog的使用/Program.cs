using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog的使用;
using SystemServices;

ServiceCollection services = new ServiceCollection();
services.AddTransient<Runner>();
services.AddTransient<Runner2>();

services.AddLogging(logBuilder =>
{
    logBuilder.AddNLog();

    /*
     *  1、NLog部分功能和.NET的Logging重复，比如分类、分级、各种Provider。
     *  2、为了避免冲突，如果用NLog，建议不要再配置.NET 的分级等（具体用法见微软文档）。
     */

    //logBuilder.SetMinimumLevel(LogLevel.Warning);
});

using (var sp = services.BuildServiceProvider())
{
    Runner runner = sp.GetRequiredService<Runner>();
    Runner2 runner2 = sp.GetRequiredService<Runner2>();

    for (int i = 0; i < 10000; i++)
    {
        runner.DoAction();
        runner2.DoAction();
    }
}