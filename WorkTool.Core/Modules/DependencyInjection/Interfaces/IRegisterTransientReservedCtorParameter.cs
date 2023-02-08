namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterTransientReservedCtorParameter
{
    void RegisterTransientReservedCtorParameter(
        ReservedCtorParameterIdentifier identifier,
        Expression expression
    );
}
