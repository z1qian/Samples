using System;

namespace ConfigService
{
    public class EnviromentConfigProvider : IConfigProvider
    {
        public string GetValue(string name)
        {
            Console.WriteLine("正在从环境变量中读取配置...");
            return Environment.GetEnvironmentVariable(name);
        }
    }
}
