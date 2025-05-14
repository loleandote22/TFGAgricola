namespace AplicacionTFG.Services;
public abstract class WebApiBase
{
    private static readonly HttpClient _client;
    static WebApiBase()
    {
        _client = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:8081/"),
        };

    }

    private HttpRequestMessage CreateRequestMessage(HttpMethod method, string url, Dictionary<string, string>? headers = null)
    {
        var httpRequestMessage = new HttpRequestMessage(method, url);
        if (headers != null && headers.Count>0)
            foreach (var header in headers)
                httpRequestMessage.Headers.Add(header.Key, header.Value);

        return httpRequestMessage;
    }


    protected async Task<string?> GetAsync(string url, Dictionary<string, string>? headers = null)
    {
        using var request = CreateRequestMessage(HttpMethod.Get, url, headers!);
        using var response = await _client.SendAsync(request);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsStringAsync();
        return null;
    }


    protected async Task<string?> PostAsync(string url, HttpContent content, Dictionary<string, string>? headers = null)
    {
        using var request = CreateRequestMessage(HttpMethod.Post, url, headers!);
        request.Content = content;
        using var response = await _client.SendAsync(request);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsStringAsync();
        return null;
    }


    protected async Task<string?> PutAsync(string url, HttpContent content, Dictionary<string, string>? headers = null)
    {
        using var request = CreateRequestMessage(HttpMethod.Put, url, headers!);
        request.Content = content;
        using var response = await _client.SendAsync(request);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsStringAsync();
        return null;
    }


    protected async Task<string?> DeleteAsync(string url, Dictionary<string, string>? headers = null)
    {
        using var request = CreateRequestMessage(HttpMethod.Delete, url, headers!);
        using var response = await _client.SendAsync(request);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsStringAsync();
        return null;
    }

}
