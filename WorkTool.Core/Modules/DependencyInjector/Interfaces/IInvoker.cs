namespace WorkTool.Core.Modules.DependencyInjector.Interfaces;

public interface IInvoker
{
    object? Invoke(Delegate @delegate, IEnumerable<ArgumentValue> arguments);
    TResult Invoke<TResult>(Delegate @delegate, IEnumerable<ArgumentValue> arguments);
    Task InvokeAsync(Delegate @delegate, IEnumerable<ArgumentValue> arguments);
    Task<TResult> InvokeAsync<TResult>(Delegate @delegate, IEnumerable<ArgumentValue> arguments);
}
