namespace WorkTool.Core.Modules.AvaloniaUi.Helpers;

public static class StyleLoaderHelper
{
    public static IEnumerable<IStyle> LoaStylesFromAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        return LoaStylesFromAssemblies(assemblies);
    }

    public static IEnumerable<IStyle> LoaStylesFromAssemblies(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var style in LoaStylesFromAssembly(assembly))
            {
                yield return style;
            }
        }
    }

    public static IEnumerable<IStyle> LoaStylesFromAssembly(Assembly assembly)
    {
        var properties = assembly.GetTypes()
            .SelectMany(x => x.GetProperties())
            .Where(
                x => x.IsStatic()
                && x.GetMethod is not null);

        var fields = assembly.GetTypes()
            .SelectMany(x => x.GetFields())
            .Where(x => x.IsStatic);

        foreach (var property in properties)
        {
            var style = property.GetCustomAttribute<StyleAttribute>();

            if (style is null)
            {
                continue;
            }

            if (style.Name.IsNullOrWhiteSpace().IsFalse())
            {
                continue;
            }

            yield return (IStyle)property.GetValue(null);
        }

        foreach (var field in fields)
        {
            var style = field.GetCustomAttribute<StyleAttribute>();

            if (style is null)
            {
                continue;
            }

            if (style.Name.IsNullOrWhiteSpace().IsFalse())
            {
                continue;
            }

            yield return (IStyle)field.GetValue(null);
        }
    }
}