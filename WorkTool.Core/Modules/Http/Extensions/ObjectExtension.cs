namespace WorkTool.Core.Modules.Http.Extensions;

public static class ObjectExtension
{
    public static HttpContent ToJsonHttpContent<TObject>(this TObject obj)
    {
        return JsonContent.Create(obj);
    }
}
