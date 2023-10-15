using System.Security.Claims;

namespace Zack.JWT;
public interface ITokenService
{
    string BuildToken(IEnumerable<Claim> claims, JWTOptions options);
}
