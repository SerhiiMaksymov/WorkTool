namespace WorkTool.Core.Modules.CommandLine.Configurations;

public readonly struct CommandLineDependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient<IApplicationCommandLine, CombineApplicationCommandLine>();
    }
}
