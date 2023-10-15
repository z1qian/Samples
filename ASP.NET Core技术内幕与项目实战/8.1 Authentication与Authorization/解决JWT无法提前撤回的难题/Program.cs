using _4.解决JWT无法提前撤回的难题;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using 解决JWT无法提前撤回的难题;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//注册与标识框架相关的服务
IServiceCollection services = builder.Services;
services.AddDbContext<TestDBContext>(opt =>
{
    string connStr = "Server=.\\SQLExpress;DataBase=IdentityDB;Trusted_Connection=True;trustServerCertificate=true;";
    opt.UseSqlServer(connStr);
});
services.AddDataProtection();
//注意我们没有调用AddIdentity方法，因为AddIdentity方法实现的初始化比较适合传统的MVC模式的项目
services.AddIdentityCore<User>(options =>
{
    //密码是否必须包含数字
    options.Password.RequireDigit = false;
    //密码是否必须包含小写字母
    options.Password.RequireLowercase = false;
    //密码是否必须包含非字母数字
    options.Password.RequireNonAlphanumeric = false;
    //密码是否必须包含大写字母
    options.Password.RequireUppercase = false;
    //密码的最小长度。默认值为6
    options.Password.RequiredLength = 6;
    //用于生成密码重置电子邮件中使用的令牌
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    //帐户确认电子邮件中使用的令牌
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
    //发生锁定时用户被锁定的时间
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    //用户被锁定之前允许的失败访问尝试的次数
    options.Lockout.MaxFailedAccessAttempts = 3;
});
var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), services);
idBuilder.AddEntityFrameworkStores<TestDBContext>()
.AddDefaultTokenProviders()
.AddRoleManager<RoleManager<Role>>()
.AddUserManager<UserManager<User>>();

//注册JWTValidationFilter到Program.cs中MVC的全局筛选器中
services.Configure<MvcOptions>(opt =>
{
    opt.Filters.Add<JWTValidationFilter>();
});

builder.Services.AddMemoryCache();

//对JWT进行配置
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(x =>
{
    byte[] keyBytes = Encoding.UTF8.GetBytes("silflsflkslflsfklsklfskd");
    var secKey = new SymmetricSecurityKey(keyBytes);
    x.TokenValidationParameters = new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = secKey
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
