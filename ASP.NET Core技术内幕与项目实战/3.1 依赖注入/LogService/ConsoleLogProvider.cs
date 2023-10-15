using System;
using System.Collections.Generic;
using System.Text;

namespace LogService
{
    internal class ConsoleLogProvider : ILogProvider
    {
        public void LogErr(string content)
        {
            Console.WriteLine($"Error:{content}");
        }

        public void LogInfo(string content)
        {
            Console.WriteLine($"Info:{content}");
        }
    }
}
