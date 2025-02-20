namespace ProxyServer.Services;

public interface IProxyClient
{
    public Task<HttpResponseMessage> GetSite(string url);
    public Task<HttpResponseMessage> PostToSite(string url, string body);
}