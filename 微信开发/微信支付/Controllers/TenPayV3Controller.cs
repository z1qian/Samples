using Microsoft.AspNetCore.Mvc;
using 微信支付.Models.ViewData;

namespace 微信支付.Controllers
{
    public class TenPayV3Controller : Controller
    {
        public IActionResult Index()
        {
            var vd = new TenPayV3_Index()
            {
                ProductList = new[] { "汉堡包", "牛排", "大鸡腿" }
            };
            return View(vd);
        }
    }
}
