using System.Collections.Specialized;

namespace WeatherAPI.Services;

public interface IWeatherClient
{
    public Task<string> GetWeather
        (string location, string? startDate, string? endDate, NameValueCollection? query);
}