using System.CommandLine;
using System.Net.Http.Headers;
using ProxyServer.Services;
using ProxyServer.Services.Caching;
using StackExchange.Redis;

string? connectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException("Redis connection string is missing.");


var portOptions = new Option<int?>(
    name: "--port",
    description: "The port to listen on.",
    getDefaultValue: () => null
);

var originOptions = new Option<string?>(
    name: "--origin",
    description: "The url to forward requests to.",
    getDefaultValue: () => null
);

var rootCommand = new RootCommand("Sets up the proxy server.");
var startCommand = new Command("caching-proxy", "Sets the server up.")
{
    portOptions,
    originOptions,
};
rootCommand.Add(startCommand);

startCommand.SetHandler(async (port, origin) =>
    {
        Console.WriteLine(port + origin);
        await SetUp(port, origin);
    },
    portOptions, originOptions);

return rootCommand.InvokeAsync(args).Result;

Task SetUp(int? port, string? origin)
{
    Environment.SetEnvironmentVariable("ASPNETCORE_URLS", $"http://localhost:{port}");
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
    Task.Run(() => app.Run());

    while (true)
    {
        var input = Console.ReadLine();
        if (string.IsNullOrEmpty(input)) continue;
        input = input.ToLower();
        if (input.StartsWith("quit")) break;
        if (!input.StartsWith("caching-proxy")) continue;
        if (!input.Contains("--clear-cache")) continue;
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect($"{connectionString},allowAdmin=true");
        var server = redis.GetServer(connectionString);
        server.FlushDatabase();
            
    }
    return Task.CompletedTask;
}
