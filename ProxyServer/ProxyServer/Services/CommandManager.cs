using StackExchange.Redis;

namespace ProxyServer.Services;

public static class CommandManager
{
    public static string? ConnectionString { get; set; }
    public static int Read()
    {
        string? input = Console.ReadLine();
        if (string.IsNullOrEmpty(input)) return 0;
        input = input.ToLower();
        if (!input.StartsWith("caching-proxy")) return 2;
        if (input.Contains("--clear-cache")) Clear();
        if (input.Contains("--close")) return 3;
        return input.Contains("--quit") ? 1 : 0;
    }

    private static void Clear()
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect($"{ConnectionString},allowAdmin=true");
        var server = redis.GetServer(ConnectionString!);
        server.FlushDatabase();
        redis.Close();
        Console.WriteLine("Cleared cache.");
    }
}