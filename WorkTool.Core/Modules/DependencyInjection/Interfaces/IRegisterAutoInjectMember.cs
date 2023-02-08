namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterAutoInjectMember
    : IRegisterTransientAutoInjectMember,
        IRegisterSingletonAutoInjectMember { }
