namespace WorkTool.Core.Modules.Http.Exceptions;

public class HttpResponseException : Exception
{
    public HttpResponseException(HttpResponseMessage httpResponseMessage)
        : this(
            httpResponseMessage.ReasonPhrase,
            httpResponseMessage.Version,
            httpResponseMessage.StatusCode,
            httpResponseMessage.Headers,
            httpResponseMessage.ReadAsStringAsync().GetAwaiter().GetResult()
        ) { }

    private HttpResponseException(
        string? reasonPhrase,
        SystemVersion version,
        HttpStatusCode statusCode,
        HttpResponseHeaders headers,
        string content
    ) : base(CreateMessage(reasonPhrase, version, statusCode, headers, content))
    {
        ReasonPhrase = reasonPhrase;
        Version = version;
        StatusCode = statusCode;
        Headers = headers;
        Content = content;
    }

    public string? ReasonPhrase { get; }
    public SystemVersion Version { get; }
    public HttpStatusCode StatusCode { get; }
    public HttpResponseHeaders Headers { get; }
    public string Content { get; }

    private static string CreateMessage(
        string? reasonPhrase,
        SystemVersion version,
        HttpStatusCode statusCode,
        HttpResponseHeaders headers,
        string content
    )
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"{statusCode} {version}");

        if (reasonPhrase is not null)
        {
            stringBuilder.Append($" {reasonPhrase}");
        }

        if (headers.Any())
        {
            foreach (var header in headers)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append($"{header.Key}: {header.Value}");
            }
        }

        if (content.Any())
        {
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(content);
        }

        var result = stringBuilder.ToString();

        return result;
    }
}
