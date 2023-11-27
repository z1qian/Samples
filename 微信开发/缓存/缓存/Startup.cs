using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.CO2NET.AspNet;
using Senparc.Weixin;
using Senparc.Weixin.Cache.Redis;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.RegisterServices;

namespace 缓存;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddMemoryCache();//使用本地缓存必须添加

        /*
         * CO2NET 是从 Senparc.Weixin 分离的底层公共基础模块，经过了长达 6 年的迭代优化，稳定可靠。
         * 关于 CO2NET 在所有项目中的通用设置可参考 CO2NET 的 Sample：
         * https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore/Startup.cs
         */

        services.AddSenparcWeixinServices(Configuration);//Senparc.Weixin 注册（必须）

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
    {

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        // 启动 CO2NET 全局注册，必须！
        // 关于 UseSenparcGlobal() 的更多用法见 CO2NET Demo：https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore3/Startup.cs
        var registerService = app.UseSenparcGlobal(env, senparcSetting.Value, globalRegister =>
        {
            #region CO2NET 全局配置

            #region 全局缓存配置（按需）

            //当同一个分布式缓存同时服务于多个网站（应用程序池）时，可以使用命名空间将其隔离（非必须）
            //globalRegister.ChangeDefaultCacheNamespace("DefaultCO2NETCache");

            #region 配置和使用 Redis          -- DPBMARK Redis

            //配置全局使用Redis缓存（按需，独立）
            if (UseRedis(senparcSetting.Value, out string redisConfigurationStr))//这里为了方便不同环境的开发者进行配置，做成了判断的方式，实际开发环境一般是确定的，这里的if条件可以忽略
            {
                /* 说明：
                 * 1、Redis 的连接字符串信息会从 Config.SenparcSetting.Cache_Redis_Configuration 自动获取并注册，如不需要修改，下方方法可以忽略
                /* 2、如需手动修改，可以通过下方 SetConfigurationOption 方法手动设置 Redis 链接信息（仅修改配置，不立即启用）
                 */
                //Senparc.CO2NET.Cache.Redis.Register.SetConfigurationOption(redisConfigurationStr);

                //以下会立即将全局缓存设置为 Redis
                Senparc.CO2NET.Cache.Redis.Register.UseKeyValueRedisNow();//键值对缓存策略（推荐）

                //Senparc.CO2NET.Cache.CsRedis.Register.UseHashRedisNow();//HashSet储存格式的缓存策略

                //也可以通过以下方式自定义当前需要启用的缓存策略
                //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisObjectCacheStrategy.Instance);//键值对
                //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisHashSetObjectCacheStrategy.Instance);//HashSet

            }
            //如果这里不进行Redis缓存启用，则目前还是默认使用内存缓存 

            #endregion                        // DPBMARK_END

            #endregion

            #endregion
        }, true)
            //使用 Senparc.Weixin SDK
            .UseSenparcWeixin(senparcWeixinSetting.Value, (weixinRegister, weixinSetting) =>
            {
                #region 微信相关配置

                /* 微信配置开始
                * 
                * 建议按照以下顺序进行注册，尤其须将缓存放在第一位！
                */

                #region 微信缓存（按需，必须放在配置开头，以确保其他可能依赖到缓存的注册过程使用正确的配置）
                //注意：如果使用非本地缓存，而不执行本块注册代码，将会收到“当前扩展缓存策略没有进行注册”的异常

                //微信的 Redis 缓存，如果不使用则注释掉（开启前必须保证配置有效，否则会抛错）         -- DPBMARK Redis
                if (UseRedis(senparcSetting.Value, out _))
                {
                    weixinRegister.UseSenparcWeixinCacheRedis();//CsRedis，两选一
                                                                //weixinRegister.UseSenparcWeixinCacheRedis();//StackExchange.Redis，两选一
                }                                                                                     // DPBMARK_END

                // DPBMARK_END

                #endregion

                #region 注册公众号或小程序（按需）

                weixinRegister
                        //注册公众号（可注册多个）                                                    -- DPBMARK MP

                        .RegisterMpAccount(senparcWeixinSetting.Value, "【子骞的测试公众号】公众号");
                // DPBMARK_END


                //除此以外，仍然可以在程序任意地方注册公众号或小程序：
                //AccessTokenContainer.Register(appId, appSecret, name);//命名空间：Senparc.Weixin.MP.Containers
                #endregion

                /* 微信配置结束 */

                #endregion
            });

        app.UseAuthorization();//需要在注册微信 SDK 之后执行
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    // -- DPBMARK Redis
    /// <summary>
    /// 判断当前配置是否满足使用 Redis（根据是否已经修改了默认配置字符串判断）
    /// </summary>
    /// <param name="senparcSetting"></param>
    /// <returns></returns>
    private bool UseRedis(SenparcSetting senparcSetting, out string redisConfigurationStr)
    {
        redisConfigurationStr = senparcSetting.Cache_Redis_Configuration;
        var useRedis = !string.IsNullOrEmpty(redisConfigurationStr) && redisConfigurationStr != "#{Cache_Redis_Configuration}#"/*默认值，不启用*/;
        return useRedis;
    }
    // -- DPBMARK_END
}