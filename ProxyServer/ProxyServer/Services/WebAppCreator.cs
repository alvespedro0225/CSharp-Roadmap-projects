using System.Net.Http.Headers;
using ProxyServer.Services.Caching;

namespace ProxyServer.Services;

public static class WebAppCreator
{
    public static async Task CreateApp(int? port, string? origin, string connectionString, Task callback)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers();
        builder.Services.AddHttpClient("Proxy", client =>
        {
            client.BaseAddress = new Uri($"https://{origin}");
            client.DefaultRequestHeaders.UserAgent.Add( new ProductInfoHeaderValue("TestApi", "0.1"));
        });

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
        });
        builder.Services.AddSingleton<IRedisCache, RedisCache>();
        builder.Services.AddSingleton<IProxyClient, ProxyClient>();
        var app = builder.Build();
        app.MapControllers();
        // runs the webapi on another thread so it doesn't block its disposing execution
        _ = app.RunAsync($"http://localhost:{port}");
        // blocks the disposing thread until it's called
        await callback;
        await app.DisposeAsync();
    }
}