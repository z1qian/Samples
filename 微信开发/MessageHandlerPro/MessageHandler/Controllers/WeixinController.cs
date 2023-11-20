using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;

namespace MessageHandler.Controllers;

[Route("[controller]")]
public class WeixinController : BaseController
{
    public static readonly string Token = MpSetting.Token;//与微信公众账号后台的Token设置保持一致，区分大小写。
    public static readonly string EncodingAESKey = MpSetting.EncodingAESKey;//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
    public static readonly string AppId = MpSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。

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
}
