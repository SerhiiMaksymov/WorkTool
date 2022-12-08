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
}
