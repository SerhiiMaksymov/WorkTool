namespace WorkTool.Core.Modules.Configuration.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister dependencyInjectorRegister)
    {
        dependencyInjectorRegister.RegisterTransient<IConfiguration, ConfigurationRoot>();

        dependencyInjectorRegister.RegisterTransient<IList<IConfigurationProvider>>(
            (JsonConfigurationProvider jsonProvider) =>
                new List<IConfigurationProvider> { jsonProvider }
        );

        dependencyInjectorRegister.RegisterTransient<IFileProvider>(
            () => new PhysicalFileProvider(SystemDirectory.GetCurrentDirectory())
        );

        dependencyInjectorRegister.RegisterTransientAutoInject(
            (JsonConfigurationSource x) => x.Path,
            () => ConfigurationConstants.DefaultFileName
        );

        dependencyInjectorRegister.RegisterTransientAutoInject(
            (JsonConfigurationSource x) => x.FileProvider
        );

        dependencyInjectorRegister.RegisterTransient<
            JsonConfigurationSource,
            JsonConfigurationSource
        >();
    }
}
