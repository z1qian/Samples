using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace OAuth_微信网页授权.Controllers;
public class OAuthController : BaseController
{
    public IActionResult Index()
    {
        var callbackUrl = "http://34f4x52061.imdo.co:5001/OAuth/Callback";

        string state = Guid.NewGuid().ToString("N");
        HttpContext.Session.SetString("State", state);

        string url = OAuthApi.GetAuthorizeUrl(AppId, callbackUrl, state, Senparc.Weixin.MP.OAuthScope.snsapi_userinfo);

        return View(model: url);
    }

    public async Task<ActionResult> CallBack(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            Senparc.Weixin.WeixinTrace.SendCustomLog("授权回调", "用户拒绝了授权");
            return Content("用户拒绝了授权");
        }

        Senparc.Weixin.WeixinTrace.SendCustomLog("授权回调", "用户同意了授权");
        var oauthTokenResult = await OAuthApi.GetAccessTokenAsync(AppId, AppSecret, code);

        string openId = oauthTokenResult.openid;
        string oauthAccessToken = oauthTokenResult.access_token;

        var userInfo = await OAuthApi.GetUserInfoAsync(oauthAccessToken, openId);

        return View(userInfo);
    }
}
