namespace WorkTool.Core.Modules.DependencyInjector.Extensions;

public static class ResolverExtension
{
    public static TObject Resolve<TObject>(this IResolver resolver)
    {
        var type = typeof(TObject);

        return (TObject)resolver.Resolve(type);
    }
}
