using Microsoft.Extensions.Configuration;
using System.Xml;

namespace 自定义配置提供者;

internal class FxConfigurationProvider : FileConfigurationProvider
{
    public FxConfigurationProvider(FileConfigurationSource source) : base(source)
    {
    }

    public override void Load(Stream stream)
    {
        //忽略大小写
        Dictionary<string, string> dicData = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(stream);

        XmlNodeList? csNodes = xmlDoc.SelectNodes("/configuration/connectionStrings/add");
        foreach (XmlNode csNode in csNodes)
        {
            string name = csNode.Attributes["name"].Value;
            string connectionString = csNode.Attributes["connectionString"].Value;
            XmlAttribute attProviderName = csNode.Attributes["providerName"];

            dicData[$"{name}:connectionString"] = connectionString;
            dicData[$"{name}:providerName"] = attProviderName?.Value ?? string.Empty;
        }

        XmlNodeList asNodes = xmlDoc.SelectNodes("/configuration/appSettings/add");
        foreach (XmlNode asNode in asNodes)
        {
            string key = asNode.Attributes["key"].Value;
            string value = asNode.Attributes["value"].Value;

            key = key.Replace(',', ':');
            dicData[$"AppSetting:{key}"] = value;
        }

        base.Data = dicData;
    }
}
