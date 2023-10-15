using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _8._4_SignalR服务器端消息推送.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class Test1Controller : ControllerBase
{
    private readonly IOptions<JWTOptions> _jwtOptions;
    private readonly IHubContext<ChatRoomHub> _hubContext;

    public Test1Controller(IOptions<JWTOptions> jwtOptions, IHubContext<ChatRoomHub> hubContext)
    {
        _jwtOptions = jwtOptions;
        _hubContext = hubContext;
    }

    [HttpPost]
    public string Login(UserLogin req)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, req.UserName),
            new Claim(ClaimTypes.NameIdentifier, req.UserId),
        };
        string jwtToken = BuildToken(claims, _jwtOptions.Value);

        return jwtToken;
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

    [HttpPost]
    public async Task<IActionResult> AddUser([FromQuery] string userName)
    {
        //这里省略执行用户注册的代码
        await _hubContext.Clients.All.SendAsync("UserAdded", userName);

        return Ok();
    }

}
