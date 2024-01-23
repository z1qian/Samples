var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSpaStaticFiles(options =>
{
    options.RootPath = "ClientApp";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseStaticFiles(new StaticFileOptions()
//{
//    RequestPath = new Microsoft.AspNetCore.Http.PathString("/static"),
//    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "clientApp/static"))
//});

app.UseStaticFiles();
app.UseSpaStaticFiles();

app.UseSpa(spa =>
{
    spa.Options.DefaultPage = "/index.html";
    spa.Options.SourcePath = "ClientApp";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
