using Senparc.NeuChar.App.AppStore;
using Senparc.NeuChar.Entities;
using Senparc.NeuChar.Entities.Request;
using Senparc.Weixin.MP.Containers;
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
    private string appId = Senparc.Weixin.Config.SenparcWeixinSetting.MpSetting.WeixinAppId;
    private string appSecret = Senparc.Weixin.Config.SenparcWeixinSetting.MpSetting.WeixinAppSecret;

    public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0, bool onlyAllowEncryptMessage = false, DeveloperInfo developerInfo = null, IServiceProvider serviceProvider = null) : base(inputStream, postModel, maxRecordCount, onlyAllowEncryptMessage, developerInfo, serviceProvider)
    {
        //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
        //比如MessageHandler<MessageContext>.GlobalGlobalMessageContext.ExpireMinutes = 3。
        GlobalMessageContext.ExpireMinutes = 3;
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
        var messageContext = await GetCurrentMessageContext();

        var handler = requestMessage.StartHandler(false)
             .Keyword("cmd", () =>
             {
                 messageContext.StorageData = new StorageModel() { IsInCmd = true };

                 var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
                 responseMessage.Content = $"cmd ok";

                 return responseMessage;
             })
             .Keywords(new string[] { "exit", "quit", "close", "退出", "leave" }, () =>
             {
                 var storageData = messageContext.StorageData as StorageModel;
                 if (storageData != null)
                 {
                     storageData.IsInCmd = false;
                 }

                 var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
                 responseMessage.Content = $"cmd {requestMessage.Content}";

                 return responseMessage;
             })
             .Regex("http", () =>
             {
                 var responseMessage = this.CreateResponseMessage<ResponseMessageNews>();

                 var news = new Article()
                 {
                     Title = "你输入了网址：" + requestMessage.Content,
                     Description = "这里是描述，第一行\r\n这里是描述，第二行",
                     PicUrl = "https://ts1.cn.mm.bing.net/th?id=ORMS.3442dae1d526dc591897392fa420b721&pid=Wdp&w=300&h=156&qlt=90&c=1&rs=1&dpr=1.5&p=0",
                     Url = requestMessage.Content
                 };

                 responseMessage.Articles.Add(news);

                 return responseMessage;
             })
             .Keyword("zxc", () =>
             {
                 var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
                 responseMessage.Content = "是否执行zxc？";

                 return responseMessage;
             })
             .Keywords(new string[] { "关机", "关" }, () =>
             {
                 string result;
                 try
                 {
                     CmdCommandRunner cmdRunner = new CmdCommandRunner();
                     result = cmdRunner.RunCmdCommand("shutdown -s -t 0");
                 }
                 catch (Exception ex)
                 {
                     result = "出现异常：" + ex.Message;
                 }

                 var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
                 responseMessage.Content = result;
                 return responseMessage;
             })
             .Default(() =>
             {
                 var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
                 responseMessage.Content = "这是一条默认的文本请求回复消息";
                 return responseMessage;
             });

        var responseMessage = handler.ResponseMessage;
        if (responseMessage is ResponseMessageText textMessage)
        {
            textMessage.Content += $"\r\n\r\n你发送了文字信息：【{requestMessage.Content}】";
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
        else if (requestMessage.EventKey == "getCount")
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();

            var messageContext = await GetCurrentMessageContext();
            var storageData = messageContext.StorageData as StorageModel;
            if (storageData != null)
            {
                responseMessage.Content = "CmdCount：" + storageData.CmdCount;
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

    public override async Task<IResponseMessageBase> OnTextOrEventRequestAsync(RequestMessageText requestMessage)
    {
        if (requestMessage.Content == "zxc")
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = $"你在OnTextOrEventRequestAsync中触发了关键字{requestMessage.Content}";

            //发送客服消息
            await Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendTextAsync(appId, OpenId,
                  "你马上要收到一条文字消息。<a href=\"https://www.baidu.com\">点击进入百度</a>\r\n这里已经换了一行\r\n这里又换了一行");

            return responseMessage;
        }

        //return null;
        return await base.OnTextOrEventRequestAsync(requestMessage);
    }

    public override async Task OnExecutingAsync(CancellationToken cancellationToken)
    {
        var messageContext = await GetCurrentMessageContext();
        var storageData = messageContext.StorageData as StorageModel;
        if (storageData != null && storageData.IsInCmd == true)
        {
            storageData.CmdCount++;

            if (storageData.CmdCount >= 5)
            {
                var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
                responseMessage.Content = "CmdCount已经 >= 5";

                ResponseMessage = responseMessage;
                CancelExecute = true;
            }
        }

        await base.OnExecutingAsync(cancellationToken);
    }

    public override Task OnExecutedAsync(CancellationToken cancellationToken)
    {
        if (ResponseMessage is ResponseMessageText textMessage)
        {
            textMessage.Content += "\r\n\r\n【子骞的测试公众号】";

            //微信请求只会等待开发者服务器响应时间5s
            //我们可以使用队列，线程处理，及时回复微信，在后台完成耗时的逻辑
        }

        return base.OnExecutedAsync(cancellationToken);
    }

    /// <summary>
    /// 关注事件
    /// </summary>
    /// <param name="requestMessage"></param>
    /// <returns></returns>
    public override async Task<IResponseMessageBase> OnEvent_SubscribeRequestAsync(RequestMessageEvent_Subscribe requestMessage)
    {
        var responseMessage = this.CreateResponseMessage<ResponseMessageText>();

        var accessToken = await AccessTokenContainer.GetAccessTokenAsync(appId);

        //var userInfo = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(accessToken, requestMessage.FromUserName);
        var userInfo = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(appId, OpenId);
        string nickName = userInfo.nickname;
        string sexName = userInfo.sex == 1 ? "先生" : (userInfo.sex == 2 ? "女士" : "未知");

        responseMessage.Content = $"你好，{nickName}{sexName}，欢迎关注【子骞的测试公众号】！";
        return responseMessage;
    }
}
