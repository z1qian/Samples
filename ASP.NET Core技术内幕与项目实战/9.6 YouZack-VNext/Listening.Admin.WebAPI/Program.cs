using CommonInitializer;
using Listening.Admin.WebAPI;
using Listening.Admin.WebAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Listening.Admin.WebAPI", Version = "v1" });
});

builder.ConfigureDefaultServices(new InitializerOptions("e:/temp/Listening.Admin.log", "Listening.Admin"));
builder.Services.AddScoped<EncodingEpisodeHelper>();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Listening.Admin.WebAPI v1"));
}

app.MapHub<EpisodeEncodingStatusHub>("/Hubs/EpisodeEncodingStatusHub");
app.UseDefaultMiddleware();
app.MapControllers();
app.Run();
