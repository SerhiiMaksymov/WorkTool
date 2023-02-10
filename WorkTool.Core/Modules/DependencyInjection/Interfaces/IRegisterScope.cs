namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterScope
{
    void RegisterScope(Type type, Expression expression);
}
