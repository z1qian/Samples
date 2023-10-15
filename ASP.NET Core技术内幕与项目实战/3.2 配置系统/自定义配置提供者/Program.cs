using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using 自定义配置提供者;

ServiceCollection services = new ServiceCollection();
//注册服务
services.AddTransient<TestWebConfig>();

ConfigurationBuilder configBuilder = new ConfigurationBuilder();
//configBuilder.Add(new FxConfigurationSource() { Path = "web.config" });
configBuilder.AddFxConfig();

IConfigurationRoot configRoot = configBuilder.Build();

services.AddOptions()
    //.Configure<WebConfig>(w => configRoot.Bind(w));
    .Configure<WebConfig>(configRoot);

using (var sp = services.BuildServiceProvider())
{
    var c = sp.GetRequiredService<TestWebConfig>();
    c.Test();
}
