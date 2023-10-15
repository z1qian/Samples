using _8._2_利用托管服务执行后台代码;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//把托管服务注册到依赖注入容器中
builder.Services.AddHostedService<DemoBgService>();
////设置程序忽略托管服务中未经处理的异常，而不是停止程序（建议不开启此设置，应该正确的处理托管服务中的异常）
//builder.Services.Configure<HostOptions>(opt =>
//{
//    opt.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
//});
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
