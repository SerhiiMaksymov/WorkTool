namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterSingletonCollection
{
    void RegisterSingletonItem(Type type, Delegate del);
}
