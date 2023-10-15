using EFCore中发布领域事件的合适时机;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Assembly assembly = Assembly.GetExecutingAssembly();
builder.Services.AddMediatR(assembly);

builder.Services.AddDbContext<UserDbContext>(opt =>
{
    string connStr = "Server=.\\SQLExpress;DataBase=DemoDB;Trusted_Connection=True;trustServerCertificate=true;";
    opt.UseSqlServer(connStr);
});

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
