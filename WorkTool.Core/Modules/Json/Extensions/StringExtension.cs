namespace WorkTool.Core.Modules.Json.Extensions;

public static class StringExtension
{
    public static JsonDocument ToJsonDocument(this string json)
    {
        return JsonDocument.Parse(json);
    }
}