using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.AspNet.HttpUtility;
using Senparc.CO2NET.Utilities;
using Senparc.NeuChar.MessageHandlers;
using Senparc.Weixin.AspNet.MvcExtension;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using SenparcClass.Service;

namespace 接口调用及数据请求.Controllers;

[Route("[controller]")]
public class WeixinController : BaseController
{
    public static readonly string Token = MpSetting.Token;//与微信公众账号后台的Token设置保持一致，区分大小写。
    public static readonly string EncodingAESKey = MpSetting.EncodingAESKey;//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
    public static readonly string AppId = MpSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。

    readonly Func<string> _getRandomFileName = () => SystemTime.Now.ToString("yyyyMMdd-HHmmss") + Guid.NewGuid().ToString("n").Substring(0, 6);

    private readonly IServiceProvider _serviceProvider;

    public WeixinController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://sdk.weixin.senparc.com/weixin
    /// </summary>
    [HttpGet]
    [ActionName("Index")]
    public ActionResult Get(PostModel postModel, string echostr)
    {
        if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
        {
            return Content(echostr); //返回随机字符串则表示验证通过
        }
        else
        {
            return Content("failed:" + postModel.Signature + "," + CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
                "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
        }
    }

    /// <summary>
    /// 【异步方法】用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
    /// </summary>
    [HttpPost]
    [ActionName("Index")]
    public async Task<ActionResult> Post(PostModel postModel)
    {
        if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
        {
            return Content("参数错误！");
        }

        #region 打包 PostModel 信息

        postModel.Token = Token;//根据自己后台的设置保持一致
        postModel.EncodingAESKey = EncodingAESKey;//根据自己后台的设置保持一致
        postModel.AppId = AppId;//根据自己后台的设置保持一致（必须提供）

        #endregion

        //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制（实际最大限制 99999）
        //注意：如果使用分布式缓存，不建议此值设置过大，如果需要储存历史信息，请使用数据库储存
        var maxRecordCount = 10;

        //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
        var messageHandler = new CustomMessageHandler(await Request.GetRequestMemoryStreamAsync(), postModel,
            maxRecordCount, serviceProvider: _serviceProvider);//接收消息（第一步）

        #region 设置消息去重设置 + 优先调用同步、异步方法设置

        /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
         * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的 RequestMessage */
        messageHandler.OmitRepeatedMessage = true;//默认已经是开启状态，此处仅作为演示，也可以设置为 false 在本次请求中停用此功能

        //当同步方法被重写，且异步方法未被重写时，尝试调用同步方法
        messageHandler.DefaultMessageHandlerAsyncEvent = DefaultMessageHandlerAsyncEvent.SelfSynicMethod;
        #endregion

        try
        {
            messageHandler.SaveRequestMessageLog();//记录 Request 日志（可选）

            var ct = new CancellationToken();
            await messageHandler.ExecuteAsync(ct);//执行微信处理过程（关键，第二步）

            messageHandler.SaveResponseMessageLog();//记录 Response 日志（可选）

            return new FixWeixinBugWeixinResult(messageHandler);//返回（第三步）
        }
        catch (Exception ex)
        {
            #region 异常处理
            WeixinTrace.Log("MessageHandler错误：{0}", ex.Message);

            using (TextWriter tw = new StreamWriter(ServerUtility.ContentRootMapPath("~/App_Data/Error_" + _getRandomFileName() + ".txt")))
            {
                tw.WriteLine("ExecptionMessage:" + ex.Message);
                tw.WriteLine(ex.Source);
                tw.WriteLine(ex.StackTrace);
                //tw.WriteLine("InnerExecptionMessage:" + ex.InnerException.Message);

                if (messageHandler.ResponseDocument != null)
                {
                    tw.WriteLine(messageHandler.ResponseDocument.ToString());
                }

                if (ex.InnerException != null)
                {
                    tw.WriteLine("========= InnerException =========");
                    tw.WriteLine(ex.InnerException.Message);
                    tw.WriteLine(ex.InnerException.Source);
                    tw.WriteLine(ex.InnerException.StackTrace);
                }

                tw.Flush();
                tw.Close();
            }
            return Content("");
            #endregion
        }
    }
}
