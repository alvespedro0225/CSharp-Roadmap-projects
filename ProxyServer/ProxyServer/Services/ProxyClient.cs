namespace ProxyServer.Services;

public sealed class ProxyClient(IHttpClientFactory httpClientFactory) : IProxyClient
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public Task<HttpResponseMessage> GetSite(string url)
    {
        var client = _httpClientFactory.CreateClient("Proxy");
        return client.GetAsync(url);
    }

    public Task<HttpResponseMessage> PostToSite(string url, string body)
    {
        var client = _httpClientFactory.CreateClient("Proxy");
        return client.PostAsJsonAsync(url, body);
    }
}