namespace 模板消息;

public static class Config
{
    public static string AppId = Senparc.Weixin.Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。

    public static Dictionary<string, List<TemplateMessageItem>> TemplateMessageCollection;

    static Config()
    {
        TemplateMessageCollection = new Dictionary<string, List<TemplateMessageItem>>();

        TemplateMessageCollection[AppId] = new List<TemplateMessageItem>()
        {
            new("测试模板消息", AppId, "fjawLZQc2OZHFqNaGomTCCZNZzY5niDItc7HeMeoi94")
        };
    }

    /// <summary>
    /// 获取TemplateMessageBag
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static TemplateMessageItem? GetTemplateMessageBag(string appId, string name)
    {
        if (!TemplateMessageCollection.TryGetValue(appId, out var list))
        {
            return null;
        }

        return list.FirstOrDefault(l => l.Name == name);
    }
}

public class TemplateMessageItem
{
    public string Name { get; set; }
    public string AppId { get; set; }
    public string TemplateId { get; set; }

    public TemplateMessageItem(string name, string appId, string templateId)
    {
        Name = name;
        AppId = appId;
        TemplateId = templateId;
    }
}
