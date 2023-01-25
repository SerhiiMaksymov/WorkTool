namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IDependencyInjector : IResolver, IInvoker, IDependencyStatusGetter
{
    ReadOnlyMemory<TypeInformation> Inputs { get; }
    ReadOnlyMemory<TypeInformation> Outputs { get; }
}
