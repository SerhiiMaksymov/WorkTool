namespace WorkTool.Core.Modules.AvaloniaUi.Helpers;

public static class ResourceLoaderHelper
{
    public static IEnumerable<IResourceProvider> LoaStylesFromAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        return LoaStylesFromAssemblies(assemblies);
    }

    public static IEnumerable<IResourceProvider> LoaStylesFromAssemblies(
        IEnumerable<Assembly> assemblies
    )
    {
        foreach (var assembly in assemblies)
        {
            foreach (var style in LoaStylesFromAssembly(assembly))
            {
                yield return style;
            }
        }
    }

    public static IEnumerable<IResourceProvider> LoaStylesFromAssembly(Assembly assembly)
    {
        var properties = assembly
            .GetTypes()
            .SelectMany(x => x.GetProperties())
            .Where(x => x.IsStatic() && x.GetMethod is not null);

        var fields = assembly.GetTypes().SelectMany(x => x.GetFields()).Where(x => x.IsStatic);

        foreach (var property in properties)
        {
            var style = property.GetCustomAttribute<ResourceAttribute>();

            if (style is null)
            {
                continue;
            }

            var value = property.GetValue(null).ThrowIfNull();

            yield return (IResourceProvider)value;
        }

        foreach (var field in fields)
        {
            var style = field.GetCustomAttribute<ResourceAttribute>();

            if (style is null)
            {
                continue;
            }

            var value = field.GetValue(null).ThrowIfNull();

            yield return (IResourceProvider)value;
        }
    }
}
