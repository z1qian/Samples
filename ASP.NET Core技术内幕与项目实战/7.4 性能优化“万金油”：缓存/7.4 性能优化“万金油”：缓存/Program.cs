var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

//如果项目启用了CORS,请确保app.UseCors写到app.UseResponseCaching之前！
//app.UseCors();

//服务区端响应缓存（不推荐使用，RFC 7234规范）
//在请求报文头中加入Cache-Control=no-cache 缓存失效
app.UseResponseCaching();

app.MapControllers();

app.Run();
