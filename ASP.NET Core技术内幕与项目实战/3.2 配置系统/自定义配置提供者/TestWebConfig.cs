using Microsoft.Extensions.Options;

namespace 自定义配置提供者;

internal class TestWebConfig
{
    private readonly IOptionsSnapshot<WebConfig> _optConfig;

    public TestWebConfig(IOptionsSnapshot<WebConfig> optConfig)
    {
        _optConfig = optConfig;
    }

    public void Test()
    {
        WebConfig cfg = _optConfig.Value;

        Console.WriteLine($"Conn1:ConnectionString={cfg.Conn1.ConnectionString}");
        Console.WriteLine($"Conn2:ConnectionString={cfg.Conn2.ConnectionString}");

        Console.WriteLine($"Conn1:ProviderName={cfg.Conn1.ProviderName}");
        Console.WriteLine($"Conn2:ProviderName={cfg.Conn2.ProviderName}");

        Console.WriteLine($"AppSetting:Name={cfg.AppSetting.Name}");
        Console.WriteLine($"AppSetting:Age={cfg.AppSetting.Age}");
        Console.WriteLine($"AppSetting:Proxy:Address={cfg.AppSetting.Proxy.Address}");
        Console.WriteLine($"AppSetting:Proxy:Port={cfg.AppSetting.Proxy.Port}");
        Console.WriteLine($"AppSetting:Proxy:Ids={string.Join(',', cfg.AppSetting.Proxy.Ids)}");
    }
}
