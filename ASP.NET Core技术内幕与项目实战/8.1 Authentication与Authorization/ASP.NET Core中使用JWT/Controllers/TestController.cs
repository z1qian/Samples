using ASP.NET_Core中使用JWT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASP.NET_Core中使用JWT.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]

public class TestController : ControllerBase
{
    [HttpPost]
    public IActionResult Login([FromServices] IOptions<JWTOptions> jwtOptions)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, "zzx"),
            //new Claim(ClaimTypes.Role, "admin"),
            new Claim(ClaimTypes.Role, "user")
        };

        string jwtToken = BuildToken(claims, jwtOptions.Value);
        return Ok(jwtToken);
    }

    private static string BuildToken(IEnumerable<Claim> claims, JWTOptions options)
    {
        DateTime expires = DateTime.Now.AddSeconds(options.ExpireSeconds);

        byte[] keyBytes = Encoding.UTF8.GetBytes(options.SigningKey);
        var secKey = new SymmetricSecurityKey(keyBytes);

        var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(expires: expires, signingCredentials: credentials, claims: claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
