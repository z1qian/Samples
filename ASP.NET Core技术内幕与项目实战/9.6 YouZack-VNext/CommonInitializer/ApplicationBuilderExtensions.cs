using Microsoft.AspNetCore.Builder;
using Zack.EventBus;

namespace CommonInitializer;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// 启用默认中间件
    /// 1.EventBus
    /// 2.Core
    /// 3.ForwardedHeaders
    /// 4.Authentication
    /// 5.Authorization
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseDefaultMiddleware(this IApplicationBuilder app)
    {
        app.UseEventBus();
        app.UseCors();

        /*
         * 由于使用nginx作反向代理服务器，请求由客户端到nginx，再有nginx转发到服务器，
         * 我们直接在服务端获取客户端IP,请求端口等信息，获取的是nginx的IP和nginx请求的端口，
         * 开启此中间件，会把客户端的IP,访问的端口等附加到请求头中，
         * 因此可以获取到客户端的真实IP和请求的端口等
         */
        app.UseForwardedHeaders();
        //不能与ForwardedHeaders很好的工作，而且webapi项目也没必要配置这个
        //app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
