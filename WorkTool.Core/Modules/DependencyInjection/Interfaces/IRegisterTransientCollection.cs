namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterTransientCollection
{
    void RegisterTransientItem(Type type, Delegate del);
}
