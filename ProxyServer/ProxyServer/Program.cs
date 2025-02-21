using System.CommandLine;
using ProxyServer.Services;

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
CommandManager.ConnectionString = connectionString;

startCommand.SetHandler(async (port, origin) =>
    {
        var app = WebAppCreator.CreateApp(port, origin, connectionString);
        _ = app.RunAsync();
        bool end = false;
        while (!end)
        {
            int response = CommandManager.Read();
            switch (response)
            {
                case 1: end = true; break;
                case 2: Console.WriteLine("Usage: caching-proxy [command]; --clear-cache, --quit"); break;
                case 3: await app.DisposeAsync(); Console.WriteLine("Closing"); break;
            }
        }
    },
    portOptions, originOptions);

return rootCommand.InvokeAsync(args).Result;