using System.Net.Http.Headers;

namespace Github_API;

public static class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: Github-API <url>");
            return;
        }

        var client = new HttpClient
        {
            BaseAddress = new Uri("https://api.github.com")
        };
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Test", "1.2.3"));
        ApiHandler apiHandler = new(client);
        var data = await apiHandler.GetData($"users/{args[0]}/events");
        if (data != null)
            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
        else
        {
            Console.WriteLine("No events found");
        }
    }
}