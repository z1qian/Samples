using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
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
            int price = productDic[name];//单位：分
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

        /// <summary>
        /// 支付回调URL，对应于TenPayV3Info.TenPayV3Notify
        /// </summary>
        /// <returns></returns>
        public IActionResult PayNotifyUrl()
        {
            try
            {
                ResponseHandler resHandler = new ResponseHandler(null);

                string returnCode = resHandler.GetParameter("return_code");
                string returnMsg = resHandler.GetParameter("return_msg");

                resHandler.SetKey(TenPayV3Info.Key);

                if (resHandler.IsTenpaySign() && returnCode.ToUpper() == "SUCCESS")
                {
                    //正确的订单处理
                    //直到这里，才能认为交易真正成功了，可以进行数据库操作，但是别忘了返回规定格式的消息！

                    /* 这里可以进行订单处理的逻辑 */

                    string appId = TenPayV3Info.AppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
                    string openId = resHandler.GetParameter("openid");

                    WeixinTrace.SendCustomLog("支付成功模板消息参数", appId + " , " + openId);
                }
                else
                {
                    //错误的订单处理
                }

                //返回给微信的请求
                RequestHandler res = new RequestHandler(null);
                //创建应答信息返回给微信
                res.SetParameter("return_code", returnCode);
                res.SetParameter("return_msg", returnMsg);

                return Content(res.ParseXML(), "text/xml");
            }
            catch (Exception ex)
            {
                new WeixinException(ex.Message, ex);
                throw;
            }
        }
    }
}
