using _4.���JWT�޷���ǰ���ص�����;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ���JWT�޷���ǰ���ص�����;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ע�����ʶ�����صķ���
IServiceCollection services = builder.Services;
services.AddDbContext<TestDBContext>(opt =>
{
    string connStr = "Server=.\\SQLExpress;DataBase=IdentityDB;Trusted_Connection=True;trustServerCertificate=true;";
    opt.UseSqlServer(connStr);
});
services.AddDataProtection();
//ע������û�е���AddIdentity��������ΪAddIdentity����ʵ�ֵĳ�ʼ���Ƚ��ʺϴ�ͳ��MVCģʽ����Ŀ
services.AddIdentityCore<User>(options =>
{
    //�����Ƿ�����������
    options.Password.RequireDigit = false;
    //�����Ƿ�������Сд��ĸ
    options.Password.RequireLowercase = false;
    //�����Ƿ�����������ĸ����
    options.Password.RequireNonAlphanumeric = false;
    //�����Ƿ���������д��ĸ
    options.Password.RequireUppercase = false;
    //�������С���ȡ�Ĭ��ֵΪ6
    options.Password.RequiredLength = 6;
    //���������������õ����ʼ���ʹ�õ�����
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    //�ʻ�ȷ�ϵ����ʼ���ʹ�õ�����
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
    //��������ʱ�û���������ʱ��
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    //�û�������֮ǰ�����ʧ�ܷ��ʳ��ԵĴ���
    options.Lockout.MaxFailedAccessAttempts = 3;
});
var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), services);
idBuilder.AddEntityFrameworkStores<TestDBContext>()
.AddDefaultTokenProviders()
.AddRoleManager<RoleManager<Role>>()
.AddUserManager<UserManager<User>>();

//ע��JWTValidationFilter��Program.cs��MVC��ȫ��ɸѡ����
services.Configure<MvcOptions>(opt =>
{
    opt.Filters.Add<JWTValidationFilter>();
});

builder.Services.AddMemoryCache();

//��JWT��������
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
