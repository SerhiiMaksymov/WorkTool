namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterTransientAutoInject
{
    public void RegisterTransientAutoInject(AutoInjectIdentifier identifier, Delegate @delegate);
}
