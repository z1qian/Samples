using Senparc.NeuChar.App.AppStore;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System.Xml.Linq;

namespace SenparcClass.Service;

/// <summary>
/// 自定义MessageHandler
/// 把MessageHandler作为基类，重写对应请求的处理方法
/// </summary>
public class CustomMessageHandler : MessageHandler<CustomMessageContext>  /*如果不需要自定义，可以直接使用：MessageHandler<DefaultMpMessageContext> */
{
    public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0, bool onlyAllowEncryptMessage = false, DeveloperInfo developerInfo = null, IServiceProvider serviceProvider = null) : base(inputStream, postModel, maxRecordCount, onlyAllowEncryptMessage, developerInfo, serviceProvider)
    {
    }

    public CustomMessageHandler(XDocument requestDocument, PostModel postModel, int maxRecordCount = 0, bool onlyAllowEncryptMessage = false, DeveloperInfo developerInfo = null, IServiceProvider serviceProvider = null) : base(requestDocument, postModel, maxRecordCount, onlyAllowEncryptMessage, developerInfo, serviceProvider)
    {
    }

    public CustomMessageHandler(RequestMessageBase requestMessageBase, PostModel postModel, int maxRecordCount = 0, bool onlyAllowEncryptMessage = false, DeveloperInfo developerInfo = null, IServiceProvider serviceProvider = null) : base(requestMessageBase, postModel, maxRecordCount, onlyAllowEncryptMessage, developerInfo, serviceProvider)
    {
    }

    public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
    {
        var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
        responseMessage.Content = $"这条消息来自DefaultResponseMessage。\r\n您收到这条消息，表明该公众号没有对【{requestMessage.MsgType}】类型做处理。";
        return responseMessage;
    }
}
