namespace WorkTool.Core.Modules.Configuration.Configurations;

public readonly struct ConfigurationDependencyInjectorConfiguration
    : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient<IConfiguration, ConfigurationRoot>();
        register.RegisterTransientAutoInject((JsonConfigurationSource x) => x.FileProvider);
        register.RegisterTransient<JsonConfigurationSource, JsonConfigurationSource>();
        register.RegisterTransient<JsonConfigurationProvider>();

        register.RegisterTransient<IList<IConfigurationProvider>>(
            (JsonConfigurationProvider jsonProvider) =>
                new List<IConfigurationProvider> { jsonProvider }
        );

        register.RegisterTransient<IFileProvider>(
            () => new PhysicalFileProvider(SystemDirectory.GetCurrentDirectory())
        );

        register.RegisterTransientAutoInject(
            (JsonConfigurationSource x) => x.Path,
            () => ConfigurationConstants.DefaultFileName
        );
    }
}
