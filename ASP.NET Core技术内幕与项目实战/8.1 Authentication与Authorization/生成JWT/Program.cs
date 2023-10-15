using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var claims = new List<Claim>
{
    //代表一条用户信息
    new Claim(ClaimTypes.NameIdentifier, "6"),
     //代表一条用户信息
    new Claim(ClaimTypes.Name, "yzk"),
    new Claim(ClaimTypes.Role, "User"),
    new Claim(ClaimTypes.Role, "Admin"),
    new Claim("PassPort", "E90000082")
};
//密钥
string key = "fasdfad&9045dafz222#fadpio@02321111";
DateTime expires = DateTime.Now.AddDays(1);
byte[] secBytes = Encoding.UTF8.GetBytes(key);
var secKey = new SymmetricSecurityKey(secBytes);
var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
var tokenDescriptor = new JwtSecurityToken(claims: claims,
    expires: expires, signingCredentials: credentials);
string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
Console.WriteLine("--------jwt--------");
Console.WriteLine(jwt);
/*
 * JWT看起来很乱，好像是加密过的，其实它们都是明文存储的，只不过进行了简单的编码而已。JWT中使用Base64URL算法对字符串进行编码
 */


string[] segments = jwt.Split('.');
string head = JwtDecode(segments[0]);
string payload = JwtDecode(segments[1]);
Console.WriteLine("--------head--------");
Console.WriteLine(head);
Console.WriteLine("--------payload--------");
Console.WriteLine(payload);
string JwtDecode(string s)
{
    s = s.Replace('-', '+').Replace('_', '/');
    switch (s.Length % 4)
    {
        case 2:
            s += "==";
            break;
        case 3:
            s += "=";
            break;
    }
    var bytes = Convert.FromBase64String(s);
    return Encoding.UTF8.GetString(bytes);
}

Console.WriteLine("--------使用密钥进行解码--------");
JwtSecurityTokenHandler tokenHandler = new();
TokenValidationParameters valParam = new();
var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
valParam.IssuerSigningKey = securityKey;
valParam.ValidateIssuer = false;
valParam.ValidateAudience = false;
//ValidateToken方法默认也会校验过期时间和JWT中的签名
ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(jwt,
        valParam, out SecurityToken secToken);
foreach (var claim in claimsPrincipal.Claims)
{
    Console.WriteLine($"{claim.Type}={claim.Value}");
}