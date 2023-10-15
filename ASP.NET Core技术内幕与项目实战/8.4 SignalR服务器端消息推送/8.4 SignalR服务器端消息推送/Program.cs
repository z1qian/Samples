using _8._4_SignalR服务器端消息推送;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//注册SingalR服务
//builder.Services.AddSignalR();

////解决SignalR部署在分布式环境中数据同步的方案（相同的集线器Hub部署在不同服务器上之间通信的问题，配置Redis）
builder.Services.AddSignalR().AddStackExchangeRedis("127.0.0.1", options =>
{
    //如果有多个SignalR应用程序连接同一台Redis服务器，那么我们需要为每一个应用程序配置唯一的ChannelPrefix
    options.Configuration.ChannelPrefix = "Test1_";
});
//配置CORS
string[] urls = new[] { "http://localhost:3000", "http://localhost:3001", "http://localhost:3002" };
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder => builder.WithOrigins(urls)
.AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

//配置JWT对连接到集线器的客户端进行身份验证
var services = builder.Services;
services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(x =>
{
    var jwtOpt = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
    byte[] keyBytes = Encoding.UTF8.GetBytes(jwtOpt.SigningKey);
    var secKey = new SymmetricSecurityKey(keyBytes);
    x.TokenValidationParameters = new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = secKey
    };
    x.Events = new JwtBearerEvents
    {
        /*
         * WebSocket不支持Authorization报文头，而且WebSocket中也不能自定义请求报文头。
         * 
         * 我们可以把JWT放到请求的URL中，然后在服务器端检测到请求的URL中有JWT，并且请求路径是针对集线器的，
         * 
         * 我们就把URL请求中的JWT取出来赋值给context.Token，接下来ASP.NET Core就能识别、解析这个JWT了
         */
        OnMessageReceived = context =>
          {
              var accessToken = context.Request.Query["access_token"];
              var path = context.HttpContext.Request.Path;
              if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/Hubs/ChatRoomHub2"))
              {
                  context.Token = accessToken;
              }
              return Task.CompletedTask;
          }
    };
});

//注册TestDBContext
services.AddDbContext<TestDBContext>(opt =>
{
    string connStr = "Server=.\\SQLExpress;DataBase=IdentityDB;Trusted_Connection=True;trustServerCertificate=true;";
    opt.UseSqlServer(connStr);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//我们需要采用前后端分离的形式编写浏览器端代码，而WebSocket的初始握手需要通过HTTP进行，所以我们需要启用CORS的支持
app.UseCors();

app.UseHttpsRedirection();

//开启身份验证，验证SignalR客户端的身份
app.UseAuthentication();

app.UseAuthorization();

//启用SignalR中间件
//并且设置当客户端通过SignalR请求“/Hubs/ChatRoomHub2”这个路径的时候，由ChatRoomHub进行处理。
app.MapHub<ChatRoomHub>("/Hubs/ChatRoomHub2");

app.MapControllers();

app.Run();