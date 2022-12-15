namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterTransient
{
    void RegisterTransient(Type type, Delegate @delegate);
}
