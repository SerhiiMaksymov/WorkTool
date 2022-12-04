namespace WorkTool.Core.Modules.Json.Extensions;

public static class ObjectExtension
{
    public static string ToJson<TObject>(this TObject obj)
    {
        return JsonSerializer.Serialize(obj);
    }
}