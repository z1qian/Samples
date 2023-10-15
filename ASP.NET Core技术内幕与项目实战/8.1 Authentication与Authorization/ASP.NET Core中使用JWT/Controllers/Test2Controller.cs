using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP.NET_Core中使用JWT.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
//在需要登录才能访问的控制器类上添加[Authorize]
//添加的[Authorize]表示这个控制器类下所有的操作方法都需要登录后才能访问。
[Authorize(Roles ="admin,user")] //允许访问资源的角色的逗号分隔列表
public class Test2Controller : ControllerBase
{
    [HttpGet]
    public IActionResult Hello()
    {
        string id = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        string userName = this.User.FindFirst(ClaimTypes.Name)!.Value;
        IEnumerable<Claim> roleClaims = this.User.FindAll(ClaimTypes.Role);
        string roleNames = string.Join(',', roleClaims.Select(c => c.Value));
        return Ok($"id={id},userName={userName},roleNames ={roleNames}");
    }

    [AllowAnonymous]
    [HttpPost]
    public string Anonymous()
    {
        return "无身份权限验证，可自由访问";
    }
}
