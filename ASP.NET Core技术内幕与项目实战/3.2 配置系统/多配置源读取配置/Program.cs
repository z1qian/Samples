using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using 多配置源读取配置;

ServiceCollection services = new ServiceCollection();
//注册服务
services.AddScoped<TestController>();
services.AddScoped<Test2>();

ConfigurationBuilder configBuilder = new ConfigurationBuilder();
//读取json配置文件
configBuilder.AddJsonFile("config.json", optional: false, reloadOnChange: true);

//读取命令行配置
configBuilder.AddCommandLine(args);

//读取环境变量配置
configBuilder.AddEnvironmentVariables("CI_");

IConfigurationRoot configRoot = configBuilder.Build();
Console.WriteLine(configRoot.GetDebugView());

services.AddOptions()
    //.Configure<Config>(c => configRoot.Bind(c))
    //.Configure<Proxy>(p => configRoot.GetSection("proxy").Bind(p));
    .Configure<Config>(configRoot)
    .Configure<Proxy>(configRoot.GetSection("proxy"));

using (var sp = services.BuildServiceProvider())
{
    while (true)
    {
        using (var scope = sp.CreateScope())
        {
            var c = scope.ServiceProvider.GetRequiredService<TestController>();
            c.Test();

            var c2 = scope.ServiceProvider.GetRequiredService<Test2>();
            c2.Test();
        }

        Console.WriteLine($"{Environment.NewLine}点击任意键继续{Environment.NewLine}");
        Console.ReadKey();
    }
}

internal class Config
{
    public string? Name { get; set; }

    public int Age { get; set; }

    public Proxy? Proxy { get; set; }
}

internal class Proxy
{
    public string? Address { get; set; }

    public int Port { get; set; }

    public int[]? Ids { get; set; }
}