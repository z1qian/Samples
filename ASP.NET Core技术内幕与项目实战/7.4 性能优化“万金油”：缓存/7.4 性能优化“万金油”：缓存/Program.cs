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

//�����Ŀ������CORS,��ȷ��app.UseCorsд��app.UseResponseCaching֮ǰ��
//app.UseCors();

//����������Ӧ���棨���Ƽ�ʹ�ã�RFC 7234�淶��
//��������ͷ�м���Cache-Control=no-cache ����ʧЧ
app.UseResponseCaching();

app.MapControllers();

app.Run();
