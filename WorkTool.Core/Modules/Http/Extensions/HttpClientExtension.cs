namespace WorkTool.Core.Modules.Http.Extensions;

public static class HttpClientExtension
{
    public static Task<HttpResponseMessage> PostAsync<TObject>(
        this HttpClient httpClient,
        string uri,
        TObject obj
    )
    {
        var content = obj.ToJsonHttpContent();

        return httpClient.PostAsync(uri, content);
    }
}
