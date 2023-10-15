using Exceptionless;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Json;
using Serilog的使用;

ExceptionlessClient.Default.Startup("06WJ8H0XtFvtYbZLvNPLS8ewPvQAFsqnk4R0Az6e");
ServiceCollection services = new ServiceCollection();
services.AddTransient<TestController>();

services.AddLogging(configure =>
{
    Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console(new JsonFormatter())
    .WriteTo.Exceptionless()
    .CreateLogger();

    configure.AddSerilog();
});

using (var sp = services.BuildServiceProvider())
{
    sp.GetRequiredService<TestController>().Test();
}

Console.WriteLine("日志写入成功");
Console.ReadLine();