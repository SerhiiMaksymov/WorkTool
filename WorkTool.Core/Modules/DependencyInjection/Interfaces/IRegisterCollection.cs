namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterCollection : IRegisterSingletonCollection, IRegisterTransientCollection
{
    void Clear(Type type);
}
