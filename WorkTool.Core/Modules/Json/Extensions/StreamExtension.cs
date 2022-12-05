namespace WorkTool.Core.Modules.Json.Extensions;

public static class StreamExtension
{
    public static Task<JsonDocument> ToJsonDocumentAsync(this Stream stream)
    {
        return JsonDocument.ParseAsync(stream);
    }
}
