namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IDependencyInjector : IResolver, IInvoker
{
    IEnumerable<Type> Inputs { get; }
    IEnumerable<Type> Outputs { get; }
}
