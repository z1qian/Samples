using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Security.Claims;

namespace 解决JWT无法提前撤回的难题;

public class JWTValidationFilter : IAsyncActionFilter
{
    private readonly IMemoryCache memCache;
    private readonly UserManager<User> userMgr;
    public JWTValidationFilter(IMemoryCache memCache, UserManager<User> userMgr)
    {
        this.memCache = memCache;
        this.userMgr = userMgr;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claimUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        //对于登录、重置密码等不需要传递Authorization报文头的接口，取到的返回值为null。这种情况下，我们把请求转给下一个筛选器。
        if (claimUserId == null)
        {
            await next();
            return;
        }
        long userId = long.Parse(claimUserId!.Value);
        string cacheKey = $"JWTValidationFilter.UserInfo.{userId}";
        User? user = await memCache.GetOrCreateAsync(cacheKey, async e =>
        {
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
            return await userMgr.FindByIdAsync(userId.ToString());
        });
        //可能用户被删除
        if (user == null)
        {
            var result = new ObjectResult($"UserId({userId}) not found");
            result.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Result = result;
            return;
        }
        var claimVersion = context.HttpContext.User.FindFirst(ClaimTypes.Version);
        long jwtVerOfReq = long.Parse(claimVersion!.Value);
        if (jwtVerOfReq >= user.JWTVersion)
        {
            await next();
        }
        else
        {
            var result = new ObjectResult($"JWTVersion mismatch");
            result.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Result = result;
            return;
        }
    }

}
