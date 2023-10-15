using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace 解决JWT无法提前撤回的难题.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public TestController(RoleManager<Role> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromQuery] string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        user.JWTVersion++;
        await _userManager.UpdateAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Version, user.JWTVersion.ToString())
        };

        string jwtToken = BuildToken(claims);
        return Ok(jwtToken);
    }

    private static string BuildToken(IEnumerable<Claim> claims)
    {
        DateTime expires = DateTime.Now.AddSeconds(120);

        byte[] keyBytes = Encoding.UTF8.GetBytes("silflsflkslflsfklsklfskd");
        var secKey = new SymmetricSecurityKey(keyBytes);

        var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(expires: expires, signingCredentials: credentials, claims: claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
