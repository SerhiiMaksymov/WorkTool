namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterSingletonAutoInjectMember
{
    void RegisterSingletonAutoInjectMember(
        AutoInjectMemberIdentifier memberIdentifier,
        Expression expression
    );
}
