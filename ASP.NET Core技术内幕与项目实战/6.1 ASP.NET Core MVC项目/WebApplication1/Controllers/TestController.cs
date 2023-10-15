using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class TestController : Controller
{
    public IActionResult Demo1()
    {
        var model = new Person("Zack", true, new DateTime(1999, 9,9));
        //请把model这个对象传递给与操作方法同名的视图
        return View(model);
    }
}
