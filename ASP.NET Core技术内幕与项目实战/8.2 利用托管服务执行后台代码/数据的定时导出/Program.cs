using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using 数据的定时导出;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TestDbContext>(opt =>
{
    string connStr = "Server=.\\SQLExpress;DataBase=IdentityDB;Trusted_Connection=True;trustServerCertificate=true;";
    opt.UseSqlServer(connStr);
});

//把托管服务注册到依赖注入容器中
builder.Services.AddHostedService<ExplortStatisticBgService>();

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
