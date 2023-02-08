namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterSingletonReservedCtorParameter
{
    void RegisterSingletonReservedCtorParameter(
        ReservedCtorParameterIdentifier identifier,
        Expression expression
    );
}
