namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterSingleton
{
    void RegisterSingleton(Type type, Expression expression);
}
