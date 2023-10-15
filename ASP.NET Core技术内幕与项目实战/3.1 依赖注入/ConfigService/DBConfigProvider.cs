using System;
using System.Collections.Generic;

namespace ConfigService
{
    public class DBConfigProvider : IConfigProvider
    {
        private readonly Dictionary<string, string> dic = new Dictionary<string, string>()
        {
            ["SmtpServer"] = "db.mail.com",
            ["Password"] = "db`123"
        };

        public string GetValue(string name)
        {
            Console.WriteLine("正在从数据库中读取配置...");

            dic.TryGetValue(name, out string value);
            return value;
        }
    }
}
