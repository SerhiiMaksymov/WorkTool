namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IRegisterAutoInject
    : IRegisterTransientAutoInject,
        IRegisterSingletonAutoInject { }
