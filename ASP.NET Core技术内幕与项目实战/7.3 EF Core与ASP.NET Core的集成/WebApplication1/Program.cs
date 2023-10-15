using BooksEFCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//对上下文进行配置的代码
//使用AddDbContext方法来通过依赖注入的方式让MyDbContext采用我们指定的连接字符串连接数据库
//builder.Services.AddDbContext<MyDbContext>(opt =>
//{
//    string connStr = builder.Configuration.GetConnectionString("Default") ?? string.Empty;
//    opt.UseSqlServer(connStr);
//});

//批量注册上下文类（上下文类采用小上下文策略）
Assembly dbContextAssembly = Assembly.Load("BooksEFCore");
builder.Services.AddAllDbContexts(opt =>
{
    string connStr = builder.Configuration.GetConnectionString("Default") ?? string.Empty;
    opt.UseSqlServer(connStr);
}, new Assembly[] { dbContextAssembly });

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
