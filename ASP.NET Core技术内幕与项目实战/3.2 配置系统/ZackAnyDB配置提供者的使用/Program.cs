using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZackAnyDB配置提供者的使用;

ServiceCollection services = new ServiceCollection();
//注册服务
services.AddScoped<TestController>();

ConfigurationBuilder configBuilder = new ConfigurationBuilder();

string connStr = "Server=.\\SQLExpress;database=CommonDB;uid=sa;password=124789hh.;trustServerCertificate=true;";
configBuilder.AddDbConfiguration(() => new SqlConnection(connStr),
    reloadOnChange: true,
    reloadInterval: TimeSpan.FromSeconds(2));
IConfigurationRoot configRoot = configBuilder.Build();

services.AddOptions()
    //.Configure<Config>(c => configRoot.Bind(c));
    .Configure<Config>(configRoot);

using (var sp = services.BuildServiceProvider())
{
    while (true)
    {
        using (var scope = sp.CreateScope())
        {
            var c = scope.ServiceProvider.GetRequiredService<TestController>();
            c.Test();

            Console.WriteLine("继续读取配置\n");
            Console.ReadKey();
        }
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