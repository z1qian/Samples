using ConfigReaderService;
using LogService;
using System;

namespace MailService
{
    public class QQMailProvider : IMailProvider
    {
        private readonly ILogProvider _log;
        private readonly IConfigReader _reader;

        public QQMailProvider(ILogProvider log, IConfigReader reader)
        {
            _log = log;
            _reader = reader;
        }

        public void Send(string title, string to, string body)
        {
            _log.LogInfo("开始发邮件啦！！！\n");

            string server= _reader.GetValue("SmtpServer");
            Console.WriteLine();
            string userName = _reader.GetValue("UserName");
            Console.WriteLine();
            string pwd = _reader.GetValue("Password");
            Console.WriteLine();
            Console.WriteLine($"邮件配置：{server}\t{userName}\t{pwd}\n");
            Console.WriteLine($"真的在发邮件！！！{title}\t{to}\t{body}\n");

            _log.LogInfo("发邮件结束！！！");
        }
    }
}
