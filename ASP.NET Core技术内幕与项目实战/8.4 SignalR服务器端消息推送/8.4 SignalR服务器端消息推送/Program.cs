using _8._4_SignalR����������Ϣ����;
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

//ע��SingalR����
//builder.Services.AddSignalR();

////���SignalR�����ڷֲ�ʽ����������ͬ���ķ�������ͬ�ļ�����Hub�����ڲ�ͬ��������֮��ͨ�ŵ����⣬����Redis��
builder.Services.AddSignalR().AddStackExchangeRedis("127.0.0.1", options =>
{
    //����ж��SignalRӦ�ó�������ͬһ̨Redis����������ô������ҪΪÿһ��Ӧ�ó�������Ψһ��ChannelPrefix
    options.Configuration.ChannelPrefix = "Test1_";
});
//����CORS
string[] urls = new[] { "http://localhost:3000", "http://localhost:3001", "http://localhost:3002" };
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder => builder.WithOrigins(urls)
.AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

//����JWT�����ӵ��������Ŀͻ��˽��������֤
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
         * WebSocket��֧��Authorization����ͷ������WebSocket��Ҳ�����Զ���������ͷ��
         * 
         * ���ǿ��԰�JWT�ŵ������URL�У�Ȼ���ڷ������˼�⵽�����URL����JWT����������·������Լ������ģ�
         * 
         * ���ǾͰ�URL�����е�JWTȡ������ֵ��context.Token��������ASP.NET Core����ʶ�𡢽������JWT��
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

//ע��TestDBContext
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

//������Ҫ����ǰ��˷������ʽ��д������˴��룬��WebSocket�ĳ�ʼ������Ҫͨ��HTTP���У�����������Ҫ����CORS��֧��
app.UseCors();

app.UseHttpsRedirection();

//���������֤����֤SignalR�ͻ��˵����
app.UseAuthentication();

app.UseAuthorization();

//����SignalR�м��
//�������õ��ͻ���ͨ��SignalR����/Hubs/ChatRoomHub2�����·����ʱ����ChatRoomHub���д���
app.MapHub<ChatRoomHub>("/Hubs/ChatRoomHub2");

app.MapControllers();

app.Run();