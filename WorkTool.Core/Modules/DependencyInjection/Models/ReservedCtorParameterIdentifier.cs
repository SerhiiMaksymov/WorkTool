namespace WorkTool.Core.Modules.DependencyInjection.Models;

public readonly record struct ReservedCtorParameterIdentifier(
    TypeInformation Type,
    ConstructorInfo Constructor,
    ParameterInfo Parameter
);
