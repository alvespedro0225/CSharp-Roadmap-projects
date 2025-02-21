using Microsoft.AspNetCore.Mvc;
using ProxyServer.Services;
using ProxyServer.Services.Caching;

namespace ProxyServer.Controllers;

[ApiController]
[Route("")]
public class ProxyController(IProxyClient client, IRedisCache cache) : Controller
{
    
    private readonly IProxyClient _proxyClient = client;
    private readonly IRedisCache _redisCache = cache;
    
    [HttpGet("/{uri}")]
    public async Task<ContentResult> Get(string uri)
    {
        var cache = await _redisCache.GetDataAsync<string?>(uri);
        if (!string.IsNullOrEmpty(cache))
        {
            Console.WriteLine("Cache: HIT");
            return new ContentResult
            {
                Content = cache,
                ContentType = "text/html"
            };
        }
        var result =  _proxyClient.GetSite($"/{uri}");
        var res = await result;
        var cont = await res.Content.ReadAsStringAsync();
        Task redis = _redisCache.SetDataAsync(uri, cont);
        Console.WriteLine("Cache: MISS");
        await redis;
        foreach (var header in res.Headers)
        {
            Response.Headers[header.Key] = header.Value.ToString();
        }
        return new ContentResult
        {
            Content = cont,
            ContentType = "text/html",
        };
    }

    [HttpGet]
    public async Task<ContentResult> Get()
    {
        var cache = await _redisCache.GetDataAsync<string?>("home");
        if (!string.IsNullOrEmpty(cache))
        {
            Console.WriteLine("Cache: HIT");
            return new ContentResult
            {
                Content = cache,
                ContentType = "text/html"
            };
        }

        var result =  _proxyClient.GetSite("");
        var res = await result;
        var cont = await res.Content.ReadAsStringAsync();
        Task redis = _redisCache.SetDataAsync("home", cont);
        Console.WriteLine("Cache: MISS");
        await redis;
        foreach (var header in res.Headers)
        {
            Response.Headers[header.Key] = header.Value.ToString();
        }
        return new ContentResult
        {
            Content = cont,
            ContentType = "text/html",
        };
    }
}