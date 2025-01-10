using System.Web;

using Microsoft.AspNetCore.Mvc;

using WeatherAPI.Services;
using WeatherAPI.Services.Caching;

namespace WeatherAPI.Controllers;

[ApiController]
[Route("[controller]/{location}")]
public class WeatherController(IWeatherClient weatherClient, IRedisCache redisCache) : Controller
{
    
    [HttpGet]
    public async Task<IActionResult> Get(string location)
    {
        var data = await redisCache.GetData<string>($"{location}");
        if (data != null) return Ok(data);
        try
        {
            var query = Request.QueryString.Value == null ?
                null:
                HttpUtility.ParseQueryString(Request.QueryString.Value);
            data = await weatherClient.GetWeather(location, null, null, query);
            redisCache.SetData($"{location}", data);
            return Ok(data);
        }
        catch (HttpRequestException e)
        {
            return Problem(statusCode:(int)e.StatusCode!);
        }
    }
    
    [HttpGet("{startDate}")]
    public async Task<IActionResult> Get(string location, string startDate)
    {
        var data = await redisCache.GetData<string>($"{location}/{startDate}");
        if (data != null) return Ok(data);
        try
        {
            var query = Request.QueryString.Value == null ?
                null:
                HttpUtility.ParseQueryString(Request.QueryString.Value);
            Console.WriteLine("\n" + query + "\n");
            data = await weatherClient.GetWeather(location, startDate, null, query);
            redisCache.SetData($"{location}", data);
            return Ok(data);
        }
        catch (HttpRequestException e)
        {
            return Problem(statusCode:(int)e.StatusCode!);
        }
    }
    
    [HttpGet("{startDate}/{endDate}")]
    public async Task<IActionResult> Get(string location, string startDate, string endDate)
    {
        var data = await redisCache.GetData<string>($"{location}/{startDate}/{endDate}");
        if (data != null) return Ok(data);
        try
        {
            var query = Request.QueryString.Value == null ?
                null:
                HttpUtility.ParseQueryString(Request.QueryString.Value);
            data = await weatherClient.GetWeather(location, startDate, endDate, query);
            redisCache.SetData($"{location}", data);
            return Ok(data);
        }
        catch (HttpRequestException e)
        {
            return Problem(statusCode:(int)e.StatusCode!);
        }
    }
}
