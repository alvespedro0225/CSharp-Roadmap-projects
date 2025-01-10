using System.Net.Http.Headers;

using WeatherAPI.Services;
using WeatherAPI.Services.Caching;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient("WeatherAPI", client =>
{
    client.BaseAddress = new Uri("https://weather.visualcrossing.com");
    client.DefaultRequestHeaders.UserAgent.Add( new ProductInfoHeaderValue("WeatherAPI", "1.0"));
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = Environment.GetEnvironmentVariable("Redis");
    options.InstanceName = "WeatherAPI";
});
builder.Services.AddControllers();
builder.Services.AddSingleton<IRedisCache, RedisCache>();
builder.Services.AddSingleton<IWeatherClient, WeatherClient>();

var app = builder.Build();
{
    app.MapControllers();
}

app.Run();
