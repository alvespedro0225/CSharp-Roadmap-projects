using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Interfaces;

namespace WeatherAPI.Controllers;

[ApiController]
public class WeatherController(IWeatherClient weatherClient) : ControllerBase
{
    [HttpGet("api/weather")]
    public async Task<IActionResult> GetWeather(string location, string startDate, string endDate)
    {
        return await weatherClient.GetWeather(location, startDate, endDate);
    }
}

