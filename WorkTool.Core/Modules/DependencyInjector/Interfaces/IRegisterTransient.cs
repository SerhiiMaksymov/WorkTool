namespace WorkTool.Core.Modules.DependencyInjector.Interfaces;

public interface IRegisterTransient
{
    void RegisterTransient<TObject, TImplementation>() where TImplementation : TObject;
    void RegisterTransient(Type type, Func<object>            func);
    void RegisterTransient(Type type, Func<IResolver, object> func);
}