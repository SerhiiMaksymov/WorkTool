namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IInvoker
{
    object? Invoke(Delegate del, DictionarySpan<Type, object> arguments);
}
