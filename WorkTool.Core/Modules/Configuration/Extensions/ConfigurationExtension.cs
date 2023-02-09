namespace WorkTool.Core.Modules.Configuration.Extensions;

public static class ConfigurationExtension
{
    public static T GetValue<T>(
        this IConfiguration configuration,
        string path,
        Func<string, T> parser,
        T def
    )
    {
        var value = configuration[path];

        if (value is null)
        {
            return def;
        }

        var result = parser.Invoke(value);

        return result;
    }
}
