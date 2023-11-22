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
        GetCurrentMessageContext().Result.ExpireMinutes = 2;
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
        responseMessage.Content = $"当前服务器时间：{DateTime.Now}";
        return responseMessage;
    }

    public override async Task<IResponseMessageBase> OnTextRequestAsync(RequestMessageText requestMessage)
    {
        var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
        responseMessage.Content = $"你输入了文字：{requestMessage.Content}";

        var messageContext = await GetCurrentMessageContext();

        if (requestMessage.Content == "cmd")
        {
            messageContext.StorageData = new StorageModel() { IsInCmd = true };
        }
        else if (requestMessage.Content == "exit")
        {
            var storageData = messageContext.StorageData as StorageModel;
            if (storageData != null)
            {
                storageData.IsInCmd = false;
            }
        }

        return responseMessage;
    }

    public override Task<IResponseMessageBase> OnLocationRequestAsync(RequestMessageLocation requestMessage)
    {
        var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
        responseMessage.Content = $"你发送了位置信息：{requestMessage.Location_X}，{requestMessage.Location_Y}";
        return Task.FromResult(responseMessage as IResponseMessageBase);
    }

    public override async Task<IResponseMessageBase> OnEvent_ClickRequestAsync(RequestMessageEvent_Click requestMessage)
    {
        if (requestMessage.EventKey == "123")
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageNews>();

            var news = new Article()
            {
                Title = "你点击了按钮：" + requestMessage.EventKey,
                Description = "这里是描述，第一行\r\n这里是描述，第二行",
                PicUrl = "https://ts1.cn.mm.bing.net/th?id=ORMS.3442dae1d526dc591897392fa420b721&pid=Wdp&w=300&h=156&qlt=90&c=1&rs=1&dpr=1.5&p=0",
                Url = "https://mp.weixin.qq.com/"
            };

            responseMessage.Articles.Add(news);

            return responseMessage;
        }
        else if (requestMessage.EventKey == "456")
        {
            ////直接不回应微信的请求
            //return null;

            //该公众号提供的服务出现故障，请稍后再试
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "";

            return responseMessage;
        }
        else if (requestMessage.EventKey == "789")
        {
            ////回应空回复
            //return Task.FromResult((IResponseMessageBase)new ResponseMessageNoResponse());

            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();

            var messageContext = await GetCurrentMessageContext();
            var storageData = messageContext.StorageData as StorageModel;
            if (storageData != null)
            {
                if (storageData.IsInCmd)
                {
                    responseMessage.Content = "已进入cmd状态";
                    responseMessage.Content += "\r\n上一条消息的类型：" + messageContext.RequestMessages.Last().MsgType;
                }
                else
                {
                    responseMessage.Content = "已退出cmd状态";
                }
            }
            else
            {
                responseMessage.Content = "未找到Session信息";
            }

            return responseMessage;
        }
        else
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = $"你点击了按钮：{requestMessage.EventKey}";

            return responseMessage;
        }
    }
}
