using Microsoft.Extensions.Options;

namespace 多配置源读取配置
{
    internal class Test2
    {
        private readonly IOptionsMonitor<Proxy> _optConfig;

        public Test2(IOptionsMonitor<Proxy> optConfig)
        {
            _optConfig = optConfig;

            Console.ForegroundColor = (ConsoleColor)Random.Shared.Next(1,16);
            Console.WriteLine($"------【{nameof(Test2)}】已被创建------");
            Console.ResetColor();
        }

        public void Test()
        {
            Proxy p = _optConfig.CurrentValue;
            Console.WriteLine($"Address:{p.Address}");
        }
    }
}
