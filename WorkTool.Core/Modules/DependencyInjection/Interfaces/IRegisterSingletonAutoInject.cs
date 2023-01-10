namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterSingletonAutoInject
{
    public void RegisterSingletonAutoInject(AutoInjectIdentifier identifier, Delegate del);
}
