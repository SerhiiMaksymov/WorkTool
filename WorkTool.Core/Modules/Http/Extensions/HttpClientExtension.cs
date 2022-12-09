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
}
