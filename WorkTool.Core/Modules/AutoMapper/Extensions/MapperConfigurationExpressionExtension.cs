using AutoMapper;

namespace WorkTool.Core.Modules.AutoMapper.Extensions;

public static class MapperConfigurationExpressionExtension
{
    public static void AddProfilesFormAssembly(
        this IMapperConfigurationExpression configuration,
        Assembly assembly
    )
    {
        var types = assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && x != typeof(Profile) && x.IsAssignableTo(typeof(Profile)));

        foreach (var type in types)
        {
            configuration.AddProfile(type);
        }
    }

    public static void AddProfilesFormAssemblies(
        this IMapperConfigurationExpression configuration,
        IEnumerable<Assembly> assemblies
    )
    {
        foreach (var assembly in assemblies)
        {
            configuration.AddProfilesFormAssembly(assembly);
        }
    }

    public static void AddProfilesFormAssemblies(this IMapperConfigurationExpression configuration)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        configuration.AddProfilesFormAssemblies(assemblies);
    }
}
