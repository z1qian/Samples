using System;
using System.IO;
using System.Linq;

namespace ConfigService
{
    public class IniConfigProvider : IConfigProvider
    {
        public string FilePath { get; set; }
        public string GetValue(string name)
        {
            Console.WriteLine("正在从ini配置文件中读取配置...");

            var list = File.ReadAllLines(FilePath).Select(line => line.Split('=')).Select(strs => new { Key = strs[0], Value = strs[1] });
            return list.SingleOrDefault(l => l.Key.Equals(name))?.Value;
        }
    }
}
