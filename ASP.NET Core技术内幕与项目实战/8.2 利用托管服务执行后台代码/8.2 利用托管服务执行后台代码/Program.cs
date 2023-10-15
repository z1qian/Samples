using _8._2_�����йܷ���ִ�к�̨����;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//���йܷ���ע�ᵽ����ע��������
builder.Services.AddHostedService<DemoBgService>();
////���ó�������йܷ�����δ��������쳣��������ֹͣ���򣨽��鲻���������ã�Ӧ����ȷ�Ĵ����йܷ����е��쳣��
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
