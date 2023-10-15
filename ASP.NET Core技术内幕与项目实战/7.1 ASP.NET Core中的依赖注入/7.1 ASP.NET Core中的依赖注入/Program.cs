using _7._1_ASP.NET_Core�е�����ע��;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//ͨ������AddControllers��������Ŀ�еĿ���������صķ���ע�ᵽ������
builder.Services.AddControllers();
builder.Services.AddScoped<MyService1>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//ͨ������AddSwaggerGen������Swagger��صķ���ע�ᵽ������
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
