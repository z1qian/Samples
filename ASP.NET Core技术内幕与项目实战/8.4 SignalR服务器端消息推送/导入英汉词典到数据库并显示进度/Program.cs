using 导入英汉词典到数据库并显示进度;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//注册SingalR服务
builder.Services.AddSignalR();

//配置CORS
string[] urls = new[] { "http://localhost:3001" };
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder => builder.WithOrigins(urls)
.AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

builder.Services.Configure<ConnStrOptions>(builder.Configuration.GetSection("ConnStrs"));
builder.Services.AddTransient<ImportExecutor>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

//启用SignalR中间件
//并且设置当客户端通过SignalR请求“/Hubs/ImportDictHub”这个路径的时候，由ImportDictHub进行处理。
app.MapHub<ImportDictHub>("/Hubs/ImportDictHub");

app.MapControllers();

app.Run();
