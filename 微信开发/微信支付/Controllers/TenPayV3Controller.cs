using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Senparc.Weixin;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.TenPay;
using Senparc.Weixin.TenPay.V3;
using 微信支付.Models.ViewData;

namespace 微信支付.Controllers
{
    public class TenPayV3Controller : Controller
    {
        private static TenPayV3Info _tenPayV3Info;

        public static TenPayV3Info TenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    var key = TenPayHelper.GetRegisterKey(Config.SenparcWeixinSetting.Items["第二个公众号"]);

                    _tenPayV3Info =
                        TenPayV3InfoCollection.Data[key];
                }
                return _tenPayV3Info;
            }
        }

        private static Dictionary<string, int> productDic = new Dictionary<string, int>()
        {
            ["汉堡包"] = 1,
            ["牛排"] = 2,
            ["大鸡腿"] = 3,
        };

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (ViewData.Model is Base_TenPayV3VD vD)
            {
                vD.Msg = nameof(OnActionExecuted);
            }

            base.OnActionExecuted(context);
        }

        public IActionResult Index()
        {
            var vd = new TenPayV3_Index()
            {
                ProductList = productDic
            };
            return View(vd);
        }

        public IActionResult Order(string name)
        {
            string body = name;
            int price = 1;//单位：分
            string openId = "";

            string orderNo = $"{TenPayV3Info.MchId}{DateTime.Now:yyyyMMddHHmmss}{TenPayV3Util.BuildRandomStr(6)}";

            JsSdkUiPackage jsPackage = JSSDKHelper.GetJsSdkUiPackage(TenPayV3Info.AppId, TenPayV3Info.AppSecret,
                HttpContext.Request.GetDisplayUrl());

            string nonceStr = jsPackage.NonceStr;
            string timeStamp = jsPackage.Timestamp;

            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(TenPayV3Info.AppId, TenPayV3Info.MchId, body, orderNo, price, HttpContext.UserHostAddress()?.ToString(), TenPayV3Info.TenPayV3Notify, TenPayV3Type.JSAPI, openId, TenPayV3Info.Key, nonceStr);

            var result = TenPayV3.Unifiedorder(xmlDataInfo);//调用统一订单接口

            var package = $"prepay_id={result.prepay_id}";

            string paySign = TenPayV3.GetJsPaySign(TenPayV3Info.AppId, timeStamp, nonceStr, package, TenPayV3Info.Key);
            var vd = new TenPayV3_Odrer()
            {
                Product = name,
                Package = package,
                JsSdkUiPackage = jsPackage,
                PaySign = paySign
            };

            //临时记录订单信息，留给退款申请接口测试使用
            HttpContext.Session.SetString("BillNo", orderNo);
            HttpContext.Session.SetString("BillFee", price.ToString());

            return View(vd);
        }
    }
}
