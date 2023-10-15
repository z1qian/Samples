using _7._1_ASP.NET_Core中的依赖注入;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//通过调用AddControllers方法把项目中的控制器及相关的服务注册到容器中
builder.Services.AddControllers();
builder.Services.AddScoped<MyService1>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//通过调用AddSwaggerGen方法把Swagger相关的服务注册到容器中
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
