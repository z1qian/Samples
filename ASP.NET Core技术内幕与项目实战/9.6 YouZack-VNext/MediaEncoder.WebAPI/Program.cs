using CommonInitializer;
using MediaEncoder.WebAPI.BgServices;
using MediaEncoder.WebAPI.Options;
using Zack.JWT;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "MediaEncoder.WebAPI", Version = "v1" });
});

builder.ConfigureDefaultServices(new InitializerOptions("e:/temp/MediaEncoder.log", "MediaEncoder.WebAPI"));

builder.Services.Configure<FileServiceOptions>(builder.Configuration.GetSection("FileService:Endpoint"));
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
builder.Services.AddHttpClient();
builder.Services.AddHostedService<EncodingBgService>();//后台转码服务
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MediaEncoder.WebAPI v1"));
}

app.UseDefaultMiddleware();
app.MapControllers();
app.Run();
