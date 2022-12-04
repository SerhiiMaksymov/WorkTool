namespace WorkTool.Core.Modules.Json.Extensions;

public static class JsonDocumentExtension
{
    public static string ToString(this JsonDocument document, JsonWriterOptions options)
    {
        using var stream = new MemoryStream();
        var       writer = new Utf8JsonWriter(stream, options);
        document.WriteTo(writer);
        writer.Flush();
        var json = Encoding.UTF8.GetString(stream.ToArray());

        return json;
    }

    public static string ToStringIndented(this JsonDocument document)
    {
        var options = new JsonWriterOptions
        {
            Indented = true
        };

        return document.ToString(options);
    }
}