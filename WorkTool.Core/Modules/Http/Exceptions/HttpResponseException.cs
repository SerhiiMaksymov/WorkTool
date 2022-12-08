namespace WorkTool.Core.Modules.Http.Exceptions;

public class HttpResponseException : Exception
{
    public HttpResponseException(HttpResponseMessage httpResponseMessage)
        : base(CreateMessage(httpResponseMessage))
    {
        StatusCode = httpResponseMessage.StatusCode;
        Version = httpResponseMessage.Version;
        ReasonPhrase = httpResponseMessage.ReasonPhrase;
        Headers = httpResponseMessage.Headers;
    }

    public string? ReasonPhrase { get; }
    public SystemVersion Version { get; }
    public HttpStatusCode StatusCode { get; }
    public HttpResponseHeaders Headers { get; }

    private static string CreateMessage(HttpResponseMessage httpResponseMessage)
    {
        return $$"""
{{httpResponseMessage.StatusCode}} {{httpResponseMessage.Version}} {{httpResponseMessage.ReasonPhrase}}
{{httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult()}}
{{httpResponseMessage.Headers.Select(x => $"{x.Key}: {x.Value}").JoinString(Environment.NewLine)}}
""";
    }
}
