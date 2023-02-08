namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterTransientAutoInjectMember
{
    void RegisterTransientAutoInjectMember(
        AutoInjectMemberIdentifier memberIdentifier,
        Expression expression
    );
}
