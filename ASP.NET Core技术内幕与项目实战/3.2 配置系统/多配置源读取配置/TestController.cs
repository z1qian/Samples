using Microsoft.Extensions.Options;

namespace 多配置源读取配置
{
    internal class TestController
    {
        private readonly IOptionsMonitor<Config> _optConfig;

        public TestController(IOptionsMonitor<Config> optConfig)
        {
            _optConfig = optConfig;

            Console.ForegroundColor = (ConsoleColor)Random.Shared.Next(1,16);
            Console.WriteLine($"------【{nameof(TestController)}】已被创建------");
            Console.ResetColor();
        }

        public void Test()
        {
            Config cfg = _optConfig.CurrentValue;
            Console.WriteLine($"Name:{cfg.Name}");
            Console.WriteLine($"Age:{cfg.Age}");
            Console.WriteLine($"Address:{cfg.Proxy!.Address}");
            Console.WriteLine($"Port:{cfg.Proxy.Port}");
            Console.WriteLine(string.Join(',', cfg.Proxy?.Ids ?? Array.Empty<int>()));
        }
    }
}
