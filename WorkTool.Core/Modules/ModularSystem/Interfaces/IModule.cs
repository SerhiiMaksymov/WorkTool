namespace WorkTool.Core.Modules.ModularSystem.Interfaces;

public interface IModule : IDependencyStatusGetter
{
    Guid Id { get; }
    ReadOnlyMemory<TypeInformation> Inputs { get; }
    ReadOnlyMemory<TypeInformation> Outputs { get; }

    object GetObject(TypeInformation type);
}
