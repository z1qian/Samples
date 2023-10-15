using BooksEFCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//�������Ľ������õĴ���
//ʹ��AddDbContext������ͨ������ע��ķ�ʽ��MyDbContext��������ָ���������ַ����������ݿ�
//builder.Services.AddDbContext<MyDbContext>(opt =>
//{
//    string connStr = builder.Configuration.GetConnectionString("Default") ?? string.Empty;
//    opt.UseSqlServer(connStr);
//});

//����ע���������ࣨ�����������С�����Ĳ��ԣ�
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
