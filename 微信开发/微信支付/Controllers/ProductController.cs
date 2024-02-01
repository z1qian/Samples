using Microsoft.AspNetCore.Mvc;

namespace 微信支付.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult H5Pay(string name)
        {
            ViewData["Name"] = name;

            return View();
        }
    }
}
