using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Zack.ASPNETCore;
using Zack.Commons;
using Zack.EventBus;
using Zack.JWT;

namespace CommonInitializer;

public static class WebApplicationBuilderExtensions
{
    private static readonly IEnumerable<Assembly> _assemblies;

    static WebApplicationBuilderExtensions()
    {
        _assemblies = ReflectionHelper.GetAllReferencedAssemblies();
    }

    /// <summary>
    /// Zack.AnyDBConfigProvider服务的注册
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder ConfigureDbConfiguration(this WebApplicationBuilder builder)
    {
        builder.Host.ConfigureAppConfiguration((hostCtx, configBuilder) =>
        {
            /*
             * 不能使用ConfigureAppConfiguration中的configBuilder去读取配置，否则就循环调用了
             * 因此这里直接自己去读取配置文件
             */
            //var configRoot = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            //string connStr = configRoot.GetValue<string>("DefaultDB:ConnStr");
            string? connStr = builder.Configuration.GetValue<string>("DefaultDB:ConnStr");
            ThrowHelper.ThorwArgumentNullExceptionIfNullOrWhiteSpace(connStr,
                "DefaultDB:ConnStr", "未设置配置项：DefaultDB:ConnStr");
            configBuilder.AddDbConfiguration(() => new SqlConnection(connStr),
                reloadOnChange: true, reloadInterval: TimeSpan.FromSeconds(5));
        });

        return builder;
    }

    /// <summary>
    /// 注册实现 IModuleInitializer 接口的模块自管理的服务
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder ConfigureModuleInitializers(this WebApplicationBuilder builder)
    {
        builder.Services.RunModuleInitializers(_assemblies);

        return builder;
    }

    /// <summary>
    /// 注册所有上下文服务（小上下文策略）
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder ConfigureAllDbContexts(this WebApplicationBuilder builder)
    {
        builder.Services.AddAllDbContexts(ctx =>
        {
            /*
             * 连接字符串如果放到appsettings.json中，会有泄密的风险
             * 如果放到UserSecrets中，每个项目都要配置，很麻烦
             * 因此这里推荐放到环境变量中
             */

            string? connStr = builder.Configuration.GetValue<string>("DefaultDB:ConnStr");
            ThrowHelper.ThorwArgumentNullExceptionIfNullOrWhiteSpace(connStr,
               "DefaultDB:ConnStr", "未设置配置项：DefaultDB:ConnStr");
            ctx.UseSqlServer(connStr);
        }, _assemblies);

        return builder;
    }

    /// <summary>
    /// 注册身份验证和鉴权服务
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder ConfigureAuthenticationAndAuthorization(this WebApplicationBuilder builder)
    {
        //只要需要校验Authentication报文头的地方（非IdentityService.WebAPI项目）也需要启用这些
        //IdentityService项目还需要启用AddIdentityCore
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication();

        JWTOptions jwtOpt = builder.Configuration.GetSection("JWT").Get<JWTOptions>()
            ?? throw new Exception("未设置配置项：JWT");
        builder.Services.AddJWTAuthentication(jwtOpt);
        //启用Swagger中的【Authorize】按钮。这样就不用每个项目的AddSwaggerGen中单独配置了
        builder.Services.Configure<SwaggerGenOptions>(c =>
        {
            c.AddAuthenticationHeader();
        });

        builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));

        return builder;
    }

    /// <summary>
    /// 注册MediatR服务
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder ConfigureMediatR(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(_assemblies);

        return builder;
    }

    /// <summary>
    /// 注册UnitOfWorkFilter
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder ConfigureUnitOfWorkFilter(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MvcOptions>(options =>
        {
            options.Filters.Add<UnitOfWorkFilter>();
        });

        return builder;
    }

    /// <summary>
    /// 设置时间格式，而非 2008-08-08T08:08:08 这样的格式
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder ConfigureDateTimeJsonConverter(this WebApplicationBuilder builder, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss")
    {
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter(dateTimeFormat));
        });

        return builder;
    }

    /// <summary>
    /// 注册CORS服务
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            /*
             * 更好的在Program.cs中用绑定方式读取配置的方法：https://github.com/dotnet/aspnetcore/issues/21491
             * 不过比较麻烦
             */
            var corsOpt = builder.Configuration.GetSection("Cors").Get<CorsSettingOptions>();
            string[] urls = corsOpt?.Origins ?? Array.Empty<string>();
            options.AddDefaultPolicy(builder => builder.WithOrigins(urls)
                    .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
        });

        return builder;
    }

    /// <summary>
    /// 注册Serilog服务
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder, InitializerOptions initOptions)
    {
        builder.Services.AddLogging(builder =>
        {
            Log.Logger = new LoggerConfiguration()
               // .MinimumLevel.Information().Enrich.FromLogContext()
               .WriteTo.Console()
               .WriteTo.File(initOptions.LogFilePath)
               .CreateLogger();
            builder.AddSerilog();
        });

        return builder;
    }

    /// <summary>
    /// 注册FluentValidation服务
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder ConfigureFluentValidation(this WebApplicationBuilder builder)
    {
        //最新注册方法
        builder.Services.AddFluentValidationAutoValidation();
        //用于把指定程序集中所有实现了IValidator接口的数据校验类注册到依赖注入容器中
        builder.Services.AddValidatorsFromAssemblies(_assemblies);
        //builder.Services.Configure<ApiBehaviorOptions>(opt =>
        //{
        //    opt.InvalidModelStateResponseFactory = context =>
        //    {
        //        var result = new
        //        {
        //            errno = 1,
        //            errmsg = string.Join(";",
        //                context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))
        //        };
        //        return new JsonResult(result) { StatusCode = StatusCodes.Status400BadRequest };
        //    };
        //});

        return builder;
    }

    /// <summary>
    /// 注册Redis服务
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder ConfigureRedis(this WebApplicationBuilder builder)
    {
        string? redisConnStr = builder.Configuration.GetValue<string>("Redis:ConnStr");
        ThrowHelper.ThorwArgumentNullExceptionIfNullOrWhiteSpace(redisConnStr,
              "Redis:ConnStr", "未设置配置项：Redis:ConnStr");

        IConnectionMultiplexer redisConnMultiplexer = ConnectionMultiplexer.Connect(redisConnStr);
        builder.Services.AddSingleton(typeof(IConnectionMultiplexer), redisConnMultiplexer);

        return builder;
    }


    /// <summary>
    /// 设置ForwardedHeaders.All
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder ConfigureForwardedHeaders(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
        });

        return builder;
    }

    /// <summary>
    /// 注册EventBus服务
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder ConfigureEventBus(this WebApplicationBuilder builder, InitializerOptions initOptions)
    {
        builder.Services.AddEventBus(initOptions.EventBusQueueName, _assemblies);
        builder.Services.Configure<IntegrationEventRabbitMQOptions>(builder.Configuration.GetSection("RabbitMQ"));

        return builder;
    }

    /// <summary>
    /// 1.ConfigureDbConfiguration
    /// 2.ConfigureModuleInitializers
    /// 3.ConfigureAllDbContexts
    /// 4.ConfigureAuthenticationAndAuthorization
    /// 5.ConfigureMediatR
    /// 6.ConfigureUnitOfWorkFilter
    /// 7.ConfigureDateTimeJsonConverter
    /// 8.ConfigureCors
    /// 9.ConfigureSerilog
    /// 10.ConfigureFluentValidation
    /// 11.ConfigureRedis
    /// 12.ConfigureForwardedHeaders
    /// 13.ConfigureEventBus
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="initOptions"></param>
    /// <returns></returns>
    public static WebApplicationBuilder ConfigureDefaultServices(this WebApplicationBuilder builder, InitializerOptions initOptions,
         string dateTimeFormat = "yyyy-MM-dd HH:mm:ss")
    {
        return builder
            .ConfigureDbConfiguration()
            .ConfigureModuleInitializers()
            .ConfigureAllDbContexts()
            .ConfigureAuthenticationAndAuthorization()
            .ConfigureMediatR()
            .ConfigureUnitOfWorkFilter()
            .ConfigureDateTimeJsonConverter(dateTimeFormat)
            .ConfigureCors()
            .ConfigureSerilog(initOptions)
            .ConfigureFluentValidation()
            .ConfigureRedis()
            .ConfigureForwardedHeaders()
            .ConfigureEventBus(initOptions);
    }
}
