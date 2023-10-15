using ����Ӣ���ʵ䵽���ݿⲢ��ʾ����;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ע��SingalR����
builder.Services.AddSignalR();

//����CORS
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

//����SignalR�м��
//�������õ��ͻ���ͨ��SignalR����/Hubs/ImportDictHub�����·����ʱ����ImportDictHub���д���
app.MapHub<ImportDictHub>("/Hubs/ImportDictHub");

app.MapControllers();

app.Run();
