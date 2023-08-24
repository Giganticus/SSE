using System.Text;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlTypes;

namespace EventProvider.Controllers;

//[ApiController]
//[Route("[controller]")]
[Route("[controller]/[action]")]
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

    [HttpGet]
    public IEnumerable<WeatherForecast> GetWeatherForecasts()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet]
    public string GetSomeOtherData()
    {
        return "some other data";
    }

    [HttpGet]
    public async Task GetEvents()
    {
        var stringsToSend = new[] { "Hello World", "Hello SSE", "Hello Again" };
        
        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");

        foreach (var stringToSend in stringsToSend)
        {
            await Task.Delay(2000);

            var eventData = $"data: {stringToSend} {DateTimeOffset.Now}";
            
            await Response.WriteAsync($"{eventData}{Environment.NewLine}");
            await Response.Body.FlushAsync();
        }
    }

    [HttpGet]
    public async Task SubscribeToProductUpdates()
    {
        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");
        
        while (true)
        {
            await Task.Delay(1000);

            await Response.WriteAsync($"data: Product Updated {DateTimeOffset.Now}{Environment.NewLine}");
            await Response.Body.FlushAsync();
        }
        
    }
}
