using System.Text.Json;

using Microsoft.Extensions.Caching.Distributed;

namespace WeatherAPI.Services.Caching;

public class RedisCache(IDistributedCache cache) : IRedisCache
{
    private readonly IDistributedCache _cache = cache;
    public async Task<T?> GetData<T>(string key)
    {
        var data = await _cache.GetStringAsync(key);
        return data == null ? 
            default :
            JsonSerializer.Deserialize<T>(data);
    }

    public async Task SetData<T>(string key, T data)
    {
        var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) }; 
        await _cache.SetStringAsync(key, JsonSerializer.Serialize(data), options);
    }
}