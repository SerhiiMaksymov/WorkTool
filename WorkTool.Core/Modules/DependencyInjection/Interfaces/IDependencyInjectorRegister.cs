namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IDependencyInjectorRegister
    : IRegisterTransient,
        IRegisterSingleton,
        IRegisterAutoInject,
        IRegisterConfiguration,
        IRegisterCollection { }
