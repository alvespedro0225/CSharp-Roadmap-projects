using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace ProxyServer.Services.Caching;

public sealed class RedisCache(IDistributedCache cache) : IRedisCache
{
    private readonly IDistributedCache _distributedCache = cache;
    public async Task<T?> GetDataAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        string? data = await _distributedCache.GetStringAsync(key, cancellationToken);
        return data == null ?
            default :
            JsonSerializer.Deserialize<T>(data);
    }

    public async Task SetDataAsync<T>(string key, T value, CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };
        string json = JsonSerializer.Serialize(value);
        await _distributedCache.SetStringAsync(key, json, options ,cancellationToken);
        // Console.WriteLine(json);
    }
}