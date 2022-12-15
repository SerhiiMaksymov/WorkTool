namespace WorkTool.Core.Modules.Http.Extensions;

public static class HttpResponseMessageExtension
{
    public static HttpResponseMessage ThrowIfNotSuccess(
        this HttpResponseMessage httpResponseMessage
    )
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            return httpResponseMessage;
        }

        throw new HttpResponseException(httpResponseMessage);
    }

    public static Task<T?> ReadFromJsonAsync<T>(this HttpResponseMessage httpResponseMessage)
    {
        return httpResponseMessage.Content.ReadFromJsonAsync<T>();
    }

    public static Task<string> ReadAsStringAsync(this HttpResponseMessage httpResponseMessage)
    {
        return httpResponseMessage.Content.ReadAsStringAsync();
    }

    public static Task<Stream> ReadAsStreamAsync(this HttpResponseMessage httpResponseMessage)
    {
        return httpResponseMessage.Content.ReadAsStreamAsync();
    }

    public static async Task<JsonDocument> ReadAsJsonDocumentAsync(
        this HttpResponseMessage httpResponseMessage
    )
    {
        await using var stream = await httpResponseMessage.ReadAsStreamAsync();
        var result = await JsonDocument.ParseAsync(stream);

        return result;
    }
}
