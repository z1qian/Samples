using Microsoft.AspNetCore.Mvc;

using Senparc.CO2NET.AspNet.HttpUtility;
using Senparc.CO2NET.Utilities;
using Senparc.Weixin;
using Senparc.Weixin.AspNet.MvcExtension;
using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP;
using Senparc.Weixin.TenPayV3;
using Senparc.Weixin.TenPayV3.Apis;
using Senparc.Weixin.TenPayV3.Apis.BasePay;
using Senparc.Weixin.TenPayV3.Helpers;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.Containers;
using Senparc.Weixin.WxOpen.Entities;
using Senparc.Weixin.WxOpen.Entities.Request;
using Senparc.Weixin.WxOpen.Helpers;

using 后端数据请求.MessageHandlers;

namespace 后端数据请求.Controllers;

public class WxOpenController : BaseController
{
    readonly Func<string> _getRandomFileName = () => SystemTime.Now.ToString("yyyyMMdd-HHmmss") + Guid.NewGuid().ToString("n").Substring(0, 6);
    /// <summary>
    /// GET请求用于处理微信小程序后台的URL验证
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ActionName("Index")]
    public ActionResult Get(PostModel postModel, string echostr)
    {
        if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, WxOpenToken))
        {
            return Content(echostr); //返回随机字符串则表示验证通过
        }
        else
        {
            return Content("failed:" + postModel.Signature + "," + CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, WxOpenToken) + "。" +
                "如果你在浏览器中看到这句话，说明此地址可以被作为微信小程序后台的Url，请注意保持Token一致。");
        }
    }


    /// <summary>
    /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
    /// </summary>
    [HttpPost]
    [ActionName("Index")]
    public async Task<ActionResult> Post(PostModel postModel, CancellationToken ctn)
    {
        if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, WxOpenToken))
        {
            return Content("参数错误！");
        }

        postModel.Token = WxOpenToken;//根据自己后台的设置保持一致
        postModel.EncodingAESKey = WxOpenEncodingAESKey;//根据自己后台的设置保持一致
        postModel.AppId = WxOpenAppId;//根据自己后台的设置保持一致（必须提供）

        //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
        var maxRecordCount = 10;

        //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
        var messageHandler = new CustomWxOpenMessageHandler(Request.GetRequestMemoryStream(), postModel, maxRecordCount);


        try
        {
            /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
             * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
            messageHandler.OmitRepeatedMessage = true;

            //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
            messageHandler.SaveRequestMessageLog();//记录 Request 日志（可选）

            await messageHandler.ExecuteAsync(ctn);//执行微信处理过程（关键）

            messageHandler.SaveResponseMessageLog();//记录 Response 日志（可选）

            //return Content(messageHandler.ResponseDocument.ToString());//v0.7-
            return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
                                                                //return new WeixinResult(messageHandler);//v0.8+
        }
        catch (Exception ex)
        {
            using (TextWriter tw = new StreamWriter(ServerUtility.ContentRootMapPath("~/App_Data/Error_WxOpen_" + _getRandomFileName() + ".txt")))
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
        }
    }

    [HttpGet]
    public ActionResult RequestData(string nickName)
    {
        var data = new
        {
            msg = $"服务器时间：{DateTime.Now}，昵称：{nickName}"
        };

        return Json(data);
    }

    /// <summary>
    /// wx.login登陆成功之后发送的请求
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> OnLogin(string code)
    {
        var result = SnsApi.JsCode2Json(WxOpenAppId, WxOpenAppSecret, code);

        if (result.errcode == ReturnCode.请求成功)
        {
            //使用SessionContainer管理登录信息（推荐）
            var sessionBag = await SessionContainer.UpdateSessionAsync(null, result.openid, result.session_key, string.Empty);

            //注意：生产环境下SessionKey属于敏感信息，不能进行传输！
            return Json(new
            {
                success = true,
                msg = "OK",
                sessionId = sessionBag.Key,
                sessionKey = sessionBag.SessionKey,/* 此参数千万不能暴露给客户端！处仅作演示！ */
                openId = sessionBag.OpenId
            });
        }
        else
        {
            return Json(new
            {
                success = false,
                msg = result.errmsg
            });
        }
    }

    /// <summary>
    /// 检查签名
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="rawData"></param>
    /// <param name="signature"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult CheckWxOpenSignature(string sessionId, string rawData, string signature)
    {
        var checkSuccess = EncryptHelper.CheckSignature(sessionId, rawData, signature);

        return Json(new
        {
            success = checkSuccess,
            msg = checkSuccess ? "签名校验成功" : "签名校验失败"
        });
    }

    /// <summary>
    /// 数据解密并进行水印校验
    /// </summary>
    /// <param name="type"></param>
    /// <param name="sessionId"></param>
    /// <param name="encryptedData"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> DecodeEncryptedData(string type, string sessionId, string encryptedData, string iv)
    {
        DecodeEntityBase? decodedEntity = null;

        switch (type.ToUpper())
        {
            case "USERINFO"://wx.getUserInfo()
                decodedEntity = EncryptHelper.DecodeUserInfoBySessionId(
                    sessionId,
                    encryptedData,
                    iv);
                break;
            default:
                break;
        }

        //检验水印
        var checkWatermark = false;
        if (decodedEntity != null)
        {
            checkWatermark = decodedEntity.CheckWatermark(WxOpenAppId);

            //保存用户信息（可选）
            if (checkWatermark && decodedEntity is DecodedUserInfo decodedUserInfo)
            {
                var sessionBag = await SessionContainer.GetSessionAsync(sessionId);
                if (sessionBag != null)
                {
                    await SessionContainer.AddDecodedUserInfoAsync(sessionBag, decodedUserInfo);
                }
            }
        }

        //注意：此处仅为演示，敏感信息请勿传递到客户端！
        return Json(new
        {
            success = checkWatermark,
            //decodedEntity = decodedEntity,
            msg = $"水印验证：{(checkWatermark ? "通过" : "不通过")}"
        });
    }

    [HttpPost]
    public async Task<ActionResult> SubscribeMessage(string sessionId, string templateId)
    {
        var sessionBag = await SessionContainer.GetSessionAsync(sessionId);
        if (sessionBag == null)
        {
            return Json(new
            {
                success = false,
                msg = "请先登录！"
            });
        }

        await Task.Delay(1000);//停1秒钟，实际开发过程中可以将权限存入数据库，任意时间发送。

        /*
         *      预约时间
                {{date3.DATA}}
                
                预约办理点
                {{thing1.DATA}}
                
                预约人
                {{name4.DATA}}
                
                联系电话
                {{phone_number11.DATA}}
                
                状态
                {{phrase13.DATA}}
         */
        var templateMessageData = new TemplateMessageData();
        templateMessageData["date3"] = new TemplateMessageDataValue(SystemTime.Now.ToString("yyyy年MM月dd日 HH:mm"));
        templateMessageData["thing1"] = new TemplateMessageDataValue(SystemTime.Now.ToString("千灯人大办公室"));
        templateMessageData["name4"] = new TemplateMessageDataValue(SystemTime.Now.ToString("子骞"));
        templateMessageData["phone_number11"] = new TemplateMessageDataValue(SystemTime.Now.ToString("13111111111"));
        templateMessageData["phrase13"] = new TemplateMessageDataValue(SystemTime.Now.ToString("已通过吧"));

        var result = await Senparc.Weixin.WxOpen.AdvancedAPIs.MessageApi.SendSubscribeAsync(WxOpenAppId, sessionBag.OpenId, templateId, templateMessageData);

        if (result.errcode == ReturnCode.请求成功)
        {
            return Json(new
            {
                success = true,
                msg = "消息已发送，请注意查收"
            });
        }
        else
        {
            return Json(new
            {
                success = false,
                msg = result.errmsg
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult> GetPrepayid(string sessionId)
    {
        try
        {
            var sessionBag = SessionContainer.GetSession(sessionId);
            if (sessionBag == null)
            {
                return Json(new
                {
                    success = false,
                    msg = "请先登录！"
                });
            }

            var openId = sessionBag.OpenId;

            //生成订单10位序列号，此处用时间和随机数生成，商户根据自己调整，保证唯一
            string sp_billno = $"{Config.SenparcWeixinSetting.TenPayV3_MchId}{DateTime.Now:yyyyMMddHHmmss}{TenPayV3Util.BuildRandomStr(6)}";

            var body = "小程序微信支付Demo";
            int price = 1;//单位：分
            string notifyUrl = Config.SenparcWeixinSetting.TenPayV3_WxOpenTenpayNotify;
            var basePayApis = new BasePayApis(Config.SenparcWeixinSetting);

            var requestData = new TransactionsRequestData(WxOpenAppId,
                Config.SenparcWeixinSetting.TenPayV3_MchId,
                body,
                sp_billno,
                new Senparc.Weixin.TenPayV3.Entities.TenpayDateTime(DateTime.Now.AddMinutes(120), false),
                HttpContext.UserHostAddress().ToString(),
                notifyUrl,
                null,
                new TransactionsRequestData.Amount() { currency = "CNY", total = price },
                new TransactionsRequestData.Payer(openId));

            var result = await basePayApis.JsApiAsync(requestData);
            var returnCode = result.ResultCode;
            if (!returnCode.Success)
            {
                return Json(new
                {
                    success = false,
                    msg = returnCode.ErrorMessage
                });
            }
            string packageStr = "prepay_id=" + result.prepay_id;

            var jsApiUiPackage = TenPaySignHelper.GetJsApiUiPackage(WxOpenAppId, result.prepay_id);

            return Json(new
            {
                success = true,
                prepay_id = result.prepay_id,
                appId = WxOpenAppId,
                timeStamp = jsApiUiPackage.Timestamp,
                nonceStr = jsApiUiPackage.NonceStr,
                packageStr = packageStr,
                signType = "RSA",
                paySign = jsApiUiPackage.Signature
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                msg = ex.Message
            });
        }
    }
}
