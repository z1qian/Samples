using _7._2_����ϵͳ��ASP.NET_Core�ļ���;
using Microsoft.Data.SqlClient;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.ConfigureAppConfiguration((_, configBuilder) =>
{
    string connStr = builder.Configuration.GetConnectionString("configServer");
    configBuilder.AddDbConfiguration(() => new SqlConnection(connStr));
});
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    //Program.cs�еĴ��벻�����������ע�������ķ�ʽ����ȡ����
    string connStr = builder.Configuration.GetValue<string>("Redis:ConnStr");
    return ConnectionMultiplexer.Connect(connStr);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
