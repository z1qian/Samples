using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//注册FluentValidation相关服务
////已被弃用
//builder.Services.AddFluentValidation(fv =>
//{
//    Assembly assembly = Assembly.GetExecutingAssembly();
//    fv.RegisterValidatorsFromAssembly(assembly);
//});
//最新注册方法
builder.Services.AddFluentValidationAutoValidation();
//用于把指定程序集中所有实现了IValidator接口的数据校验类注册到依赖注入容器中
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.InvalidModelStateResponseFactory = context =>
    {
        var result = new
        {
            errno = 1,
            errmsg = string.Join(";",
                context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))
        };
        return new JsonResult(result) { StatusCode = StatusCodes.Status400BadRequest };
    };
});
//添加Fluent验证客户端适配器（WebApi项目不需要开启）
//builder.Services.AddFluentValidationClientsideAdapters();

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
