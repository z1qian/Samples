using Microsoft.AspNetCore.Mvc;

namespace _6._2_使用ASP.NET_Core开发Web_API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public string SaveNote(SaveNoteRequest req)
    {
        string filename = $"{req.Title}.txt";
        System.IO.File.WriteAllText(filename, req.Content);

        return filename;
    }

    //[HttpPut]
    //[ApiExplorerSettings(IgnoreApi = true)] //告知Swagger忽略这个方法
    //public string TestMethod()
    //{
    //    return "Hello";
    //}

    /*
     * 通过不同的路由配置，ASP.NET Core支持在同一个控制器中存在同名的重载操作方法
     * 但由于配置不当，可能会导致我们认为应该访问A1的请求，访问了A2
     * 所以，我们推荐不同的操作，要有不用的方法名，方法名要不同
     */
    [HttpPut]
    public string MethodOne()
    {
        return "1";
    }

    [Route("api")]
    [HttpPut]
    public string MethodOne(string content)
    {
        return content;
    }
}