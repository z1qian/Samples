using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ��ʶ��ܵ�ʹ��;
using ��ʶ��ܵ�ʹ��.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ע�����ʶ�����صķ���
IServiceCollection services = builder.Services;
services.AddDbContext<IdDbContext>(opt =>
{
    string connStr = builder.Configuration.GetConnectionString("Default");
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
