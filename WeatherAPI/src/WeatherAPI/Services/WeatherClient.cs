using System.Collections.Specialized;

namespace WeatherAPI.Services;

public class WeatherClient(IHttpClientFactory httpClientFactory) : IWeatherClient
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly string _apiKey = Environment.GetEnvironmentVariable("ApiKey")!;
    
    public async Task<string> GetWeather
        (string location, string? startDate, string? endDate, NameValueCollection? query)
    {
        var httpClient = _httpClientFactory.CreateClient("WeatherAPI");
        var uri = $"/VisualCrossingWebServices/rest/services/timeline/{location}";
        if (!string.IsNullOrEmpty(startDate)) uri += $"/{startDate}";
        if (!string.IsNullOrEmpty(endDate)) uri += $"/{endDate}";
        uri += $"?key={_apiKey}";
        if (query != null)
        {
            var keys = query.AllKeys;
            for (var i = 0; i < keys.Length; i++)
            {
               uri += $"&{keys[i]}={query[keys[i]]}";
            }
        }


        Console.WriteLine("\n" + uri + "\n");
        return await httpClient.GetStringAsync(uri);
    }
}