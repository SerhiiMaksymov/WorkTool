namespace WorkTool.Core.Modules.ModularSystem.Interfaces;

public interface IModule
{
    IDependencyInjector DependencyInjector { get; }

    IModule Join(IModule module);
}
