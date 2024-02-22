using Microsoft.AspNetCore.Mvc;

namespace 后端数据请求.Controllers
{
    public class WxOpenController : Controller
    {

        [HttpGet]
        public ActionResult RequestData(string nickName)
        {
            var data = new
            {
                msg = $"服务器时间：{DateTime.Now}，昵称：{nickName}"
            };

            return Json(data);
        }
    }
}
