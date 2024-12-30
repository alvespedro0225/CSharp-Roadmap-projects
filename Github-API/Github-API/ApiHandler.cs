using System.Text.Json;

namespace Github_API;

public class ApiHandler(HttpClient client)
{
    private readonly HttpClient _httpClient = client;
    private readonly JsonSerializerOptions _settings = new()
    {
        PropertyNameCaseInsensitive = true
    };
    public async Task<List<GithubEvents>?> GetData(string url)
    {
        try
        { 
            var response = await _httpClient.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<List<GithubEvents>>(response, _settings);
            return data;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}