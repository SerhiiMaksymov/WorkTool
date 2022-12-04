namespace WorkTool.SourceGenerator.Extensions;

public static class ObjectExtension
{
    public static TObject ThrowIfNull<TObject>(this TObject obj, string name)
    {
        return obj ?? throw new ArgumentNullException(name);
    }
}