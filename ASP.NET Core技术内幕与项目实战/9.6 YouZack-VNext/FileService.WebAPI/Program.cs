using CommonInitializer;
using FileService.Infratructure.StorageServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureDefaultServices(new InitializerOptions("e:/temp/FileService.log", "FileService.WebAPI"));
builder.Services//.AddOptions() //asp.net core��Ŀ��AddOptions()��дҲ�У���Ϊ���һ���Զ�ִ����
    .Configure<SMBStorageOptions>(builder.Configuration.GetSection("FileService:SMB"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
    s.SwaggerDoc("v1", new () { Title = "FileService.WebAPI", Version = "v1" }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FileService.WebAPI v1"));
}

app.UseStaticFiles();
app.UseDefaultMiddleware();

app.MapControllers();

app.Run();
