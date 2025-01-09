using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Interfaces;

public interface IWeatherClient
{
    public Task<IActionResult> GetWeather(string city, string? startDate=null, string? endDate=null);
    
}