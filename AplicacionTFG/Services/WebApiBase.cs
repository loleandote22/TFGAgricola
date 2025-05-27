namespace AplicacionTFG.Services;
public abstract class WebApiBase
{
    private readonly HttpClient _client;
    public WebApiBase(string url)
    {
        _client = new HttpClient()
        {
            BaseAddress = new Uri("https://280d-93-187-135-67.ngrok-free.app"),
            //BaseAddress = new Uri("https://localhost:7266/"), // Cambia esto a la URL de tu API
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
        var response = await _client.SendAsync(request);
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
