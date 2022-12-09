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
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"{httpResponseMessage.StatusCode} {httpResponseMessage.Version}");

        if (httpResponseMessage.ReasonPhrase is not null)
        {
            stringBuilder.Append($" {httpResponseMessage.ReasonPhrase}");
        }

        if (httpResponseMessage.Headers.Any())
        {
            foreach (var header in httpResponseMessage.Headers)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append($"{header.Key}: {header.Value}");
            }
        }

        var content = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        if (content.Any())
        {
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(content);
        }

        var result = stringBuilder.ToString();

        return result;
    }
}
