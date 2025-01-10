namespace WeatherAPI.Services.Caching;

public interface IRedisCache
{
    public Task<T?> GetData<T>(string key);
    public void SetData<T>(string key, T data);
}