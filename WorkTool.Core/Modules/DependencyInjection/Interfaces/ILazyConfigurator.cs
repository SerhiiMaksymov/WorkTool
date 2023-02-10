namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface ILazyConfigurator
{
    void SetLazyOptions(TypeInformation type, LazyDependencyInjectorOptions options);
}
