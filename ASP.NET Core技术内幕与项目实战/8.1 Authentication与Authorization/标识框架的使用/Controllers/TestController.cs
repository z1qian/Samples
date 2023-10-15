using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using 标识框架的使用.Models;

namespace 标识框架的使用.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public TestController(ILogger<TestController> logger, RoleManager<Role> roleManager, UserManager<User> userManager)
    {
        _logger = logger;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<ActionResult<bool>> CreateUserRole()
    {
        bool roleExists = await _roleManager.RoleExistsAsync("admin");
        if (!roleExists)
        {
            Role role = new Role { Name = "Admin" };
            var r = await _roleManager.CreateAsync(role);
            if (!r.Succeeded)
            {
                return BadRequest(r.Errors);
            }
        }
        User user = await _userManager.FindByNameAsync("yzk");
        if (user == null)
        {
            user = new User { UserName = "yzk", Email = "2947417074@qq.com", EmailConfirmed = true };
            var r = await _userManager.CreateAsync(user, "123456");
            if (!r.Succeeded)
            {
                return BadRequest(r.Errors);
            }
            r = await _userManager.AddToRoleAsync(user, "admin");
            if (!r.Succeeded)
            {
                return BadRequest(r.Errors);
            }
        }

        return true;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        string userName = req.UserName;
        string password = req.Password;

        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
            return NotFound($"用户名不存在{userName}");

        if (await _userManager.IsLockedOutAsync(user))
            return BadRequest("LockedOut");

        var success = await _userManager.CheckPasswordAsync(user, password);
        if (success)
        {
            await _userManager.ResetAccessFailedCountAsync(user);
            return Ok("Success");
        }
        else
        {
            await _userManager.AccessFailedAsync(user);
            return BadRequest("账号或密码错误");
        }
    }

    [HttpGet]
    public async Task<ActionResult<bool>> SendResetPasswordToken([FromQuery] string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        //生成密码重置令牌
        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        _logger.LogInformation($"向邮箱{user.Email}发送Token={token}");

        return true;
    }

    [HttpPost]
    public async Task<ActionResult<bool>> VerifyResetPasswordToken(VerifyResetPasswordTokenRequest req)
    {
        string email = req.Email;
        var user = await _userManager.FindByEmailAsync(email);

        string token = req.Token;
        string password = req.NewPassword;

        var r = await _userManager.ResetPasswordAsync(user, token, password);

        return r.Succeeded;
    }
}
