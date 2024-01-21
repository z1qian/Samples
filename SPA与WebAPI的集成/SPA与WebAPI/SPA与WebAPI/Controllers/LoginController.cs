using Microsoft.AspNetCore.Mvc;

namespace SPA与WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
    public ActionResult Index()
    {
        return Content("ok");
    }
}
