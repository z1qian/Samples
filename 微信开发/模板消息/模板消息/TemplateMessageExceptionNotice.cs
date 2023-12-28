using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

namespace 模板消息;

//对应一个消息模板
public class TemplateMessageExceptionNotice : TemplateMessageBase
{
    public TemplateMessageExceptionNotice(string appName, string exception, string type, string errCode, string url)
        : base("EWdTMhwW2gky_MIDA6jOh9E4PwAlKxt_zqzqiKpVUJs", url, "系统异常通知")
    {
        keyword1 = new TemplateDataItem(appName);
        keyword2 = new TemplateDataItem(exception);
        keyword3 = new TemplateDataItem(type);
        keyword4 = new TemplateDataItem(errCode);
    }

    public TemplateDataItem keyword1 { get; set; }
    public TemplateDataItem keyword2 { get; set; }
    public TemplateDataItem keyword3 { get; set; }
    public TemplateDataItem keyword4 { get; set; }
}
