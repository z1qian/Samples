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

//ע��FluentValidation��ط���
////�ѱ�����
//builder.Services.AddFluentValidation(fv =>
//{
//    Assembly assembly = Assembly.GetExecutingAssembly();
//    fv.RegisterValidatorsFromAssembly(assembly);
//});
//����ע�᷽��
builder.Services.AddFluentValidationAutoValidation();
//���ڰ�ָ������������ʵ����IValidator�ӿڵ�����У����ע�ᵽ����ע��������
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
//���Fluent��֤�ͻ�����������WebApi��Ŀ����Ҫ������
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
