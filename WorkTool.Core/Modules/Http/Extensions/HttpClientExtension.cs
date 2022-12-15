namespace WorkTool.Core.Modules.Http.Extensions;

public static class HttpClientExtension
{
    public static async Task<HttpResponseMessage> PostAsync<TObject>(
        this HttpClient httpClient,
        string uri,
        TObject obj
    )
    {
        using var content = obj.ToJsonHttpContent();

        return await httpClient.PostAsync(uri, content);
    }

    public static async Task<JsonDocument> PostReadJsonDocumentThrowIfNotSuccessAsync<T>(
        this HttpClient httpClient,
        string url,
        T obj
    )
    {
        using var jsonContent = JsonContent.Create(obj);
        using var httpResponseMessage = await httpClient.PostAsync(url, jsonContent);
        httpResponseMessage.ThrowIfNotSuccess();
        var result = await httpResponseMessage.ReadAsJsonDocumentAsync();

        return result;
    }

    public static async Task<T?> PostReadJsonThrowIfNotSuccessAsync<T>(
        this HttpClient httpClient,
        string url,
        HttpContent httpContent
    )
    {
        using var httpResponseMessage = await httpClient.PostAsync(url, httpContent);
        httpResponseMessage.ThrowIfNotSuccess();
        var result = await httpResponseMessage.ReadFromJsonAsync<T>();

        return result;
    }

    public static async Task<T?> PostReadJsonThrowIfNotSuccessAsync<T>(
        this HttpClient httpClient,
        string url,
        object obj
    )
    {
        using var jsonContent = JsonContent.Create(obj);
        using var httpResponseMessage = await httpClient.PostAsync(url, jsonContent);
        httpResponseMessage.ThrowIfNotSuccess();
        var result = await httpResponseMessage.ReadFromJsonAsync<T>();

        return result;
    }

    public static async Task<T?> PostReadJsonThrowIfNotSuccessAsync<T>(
        this HttpClient httpClient,
        string url
    )
    {
        await using var memoryStream = new MemoryStream();
        using var streamContent = new StreamContent(memoryStream);
        var result = await httpClient.PostReadJsonThrowIfNotSuccessAsync<T>(url, streamContent);

        return result;
    }

    public static HttpClient SetAuthorization(
        this HttpClient httpClient,
        AuthenticationHeaderValue? authorization
    )
    {
        httpClient.DefaultRequestHeaders.SetAuthorization(authorization);

        return httpClient;
    }
}
