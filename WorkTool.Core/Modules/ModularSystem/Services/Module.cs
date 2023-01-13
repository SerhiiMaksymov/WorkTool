using WorkTool.Core.Modules.ModularSystem.Interfaces;

namespace WorkTool.Core.Modules.ModularSystem.Services;

public class Module : IModule
{
    public IDependencyInjector DependencyInjector { get; }

    public Module(IDependencyInjector dependencyInjector)
    {
        DependencyInjector = dependencyInjector;
    }

    public IModule Join(IModule module)
    {
        var dependencyInjector = new JoinDependencyInjector(
            module.DependencyInjector,
            DependencyInjector
        );

        var result = new Module(dependencyInjector);

        return result;
    }
}
