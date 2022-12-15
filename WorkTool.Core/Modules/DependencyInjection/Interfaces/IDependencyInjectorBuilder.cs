namespace WorkTool.Core.Modules.DependencyInjection.Interfaces;

public interface IDependencyInjectorBuilder
    : IRegisterTransient,
        IRegisterSingleton,
        IRegisterRegisterRegisterReserve,
        IRegisterAutoInject { }
