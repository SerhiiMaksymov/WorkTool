namespace WorkTool.Core.Modules.EntityFrameworkCore.Extensions;

public static class ModelBuilderExtension
{
    public static void ApplyConfigurationsFromAssemblies(
        this ModelBuilder modelBuilder,
        IEnumerable<Assembly> assemblies
    )
    {
        foreach (var assembly in assemblies)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }

    public static void ApplyConfigurationsFromAssemblies(this ModelBuilder modelBuilder)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        modelBuilder.ApplyConfigurationsFromAssemblies(assemblies);
    }
}
