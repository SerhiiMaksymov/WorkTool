namespace WorkTool.Core.Modules.DependencyInjector.Extensions;

public static class ObjectExtension
{
    public static ArgumentValue<TObject> ToArgumentValue<TObject>(this TObject obj)
    {
        return new ArgumentValue<TObject>(obj);
    }
}
