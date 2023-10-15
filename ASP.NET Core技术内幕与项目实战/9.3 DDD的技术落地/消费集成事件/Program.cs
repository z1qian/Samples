using System.Reflection;
using Zack.EventBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//消费方EventBus配置1
var eventBusSec = builder.Configuration.GetSection("EventBus");
builder.Services.Configure<IntegrationEventRabbitMQOptions>(eventBusSec);
builder.Services.AddEventBus(/*程序绑定的队列的名字,一般每个微服务项目的queueName参数值都不同，以便每个程序都能收到消息*/"EventBusDemo1_Q2",
    /*含有监听集成事件的处理者代码的程序集*/Assembly.GetExecutingAssembly());

var app = builder.Build();

//消费方EventBus配置2
app.UseEventBus();

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
