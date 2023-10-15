using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using 标识框架的使用;
using 标识框架的使用.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//注册与标识框架相关的服务
IServiceCollection services = builder.Services;
services.AddDbContext<IdDbContext>(opt =>
{
    string connStr = builder.Configuration.GetConnectionString("Default");
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
idBuilder.AddEntityFrameworkStores<IdDbContext>()
.AddDefaultTokenProviders()
.AddRoleManager<RoleManager<Role>>()
.AddUserManager<UserManager<User>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
