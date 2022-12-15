namespace WorkTool.Core.Modules.Configuration.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorBuilder dependencyInjectorBuilder)
    {
        dependencyInjectorBuilder.RegisterTransient<IConfiguration, ConfigurationRoot>();

        dependencyInjectorBuilder.RegisterTransient<IList<IConfigurationProvider>>(
            (JsonConfigurationProvider jsonProvider) =>
                new List<IConfigurationProvider> { jsonProvider }
        );

        dependencyInjectorBuilder.RegisterTransient<IFileProvider>(
            () => new PhysicalFileProvider(Directory.GetCurrentDirectory())
        );

        dependencyInjectorBuilder.RegisterTransientAutoInject(
            (JsonConfigurationSource x) => x.Path,
            () => ConfigurationConstants.DefaultFileName
        );

        dependencyInjectorBuilder.RegisterTransientAutoInject(
            (JsonConfigurationSource x) => x.FileProvider
        );

        dependencyInjectorBuilder.RegisterTransient<
            JsonConfigurationSource,
            JsonConfigurationSource
        >();
    }
}
