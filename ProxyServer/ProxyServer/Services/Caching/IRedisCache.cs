namespace ProxyServer.Services.Caching;

public interface IRedisCache
{
    public Task<T?> GetDataAsync<T>(string key, CancellationToken cancellationToken = default);
    public Task SetDataAsync<T>(string key, T value, CancellationToken cancellationToken = default);
}