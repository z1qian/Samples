using CommonInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Listening.Main.WebAPI", Version = "v1" });
});

builder.ConfigureDefaultServices(new InitializerOptions("e:/temp/Listening.Main.log", "Listening.Main"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Listening.Main.WebAPI v1"));
}

app.UseDefaultMiddleware();
app.MapControllers();
app.Run();
