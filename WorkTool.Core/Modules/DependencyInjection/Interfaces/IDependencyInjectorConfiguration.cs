namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IDependencyInjectorConfiguration
{
    void Configure(IDependencyInjectorRegister dependencyInjectorRegister);
}
