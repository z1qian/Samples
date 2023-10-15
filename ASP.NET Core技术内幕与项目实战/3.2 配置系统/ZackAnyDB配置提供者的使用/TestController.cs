using Microsoft.Extensions.Options;

namespace ZackAnyDB配置提供者的使用
{
    internal class TestController
    {
        private readonly IOptionsMonitor<Config> _optConfig;

        public TestController(IOptionsMonitor<Config> optConfig)
        {
            _optConfig = optConfig;

            Console.WriteLine($"{nameof(TestController)}已被创建");
        }

        public void Test()
        {
            Config cfg = _optConfig.CurrentValue;

            Console.WriteLine($"Name:{cfg.Name}");
            Console.WriteLine($"Age:{cfg.Age}");
            Console.WriteLine($"Address:{cfg.Proxy!.Address}");
            Console.WriteLine($"Port:{cfg.Proxy.Port}");
            Console.WriteLine(string.Join(',', cfg.Proxy.Ids));
        }
    }
}
