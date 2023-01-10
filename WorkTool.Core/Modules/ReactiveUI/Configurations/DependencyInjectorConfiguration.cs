namespace WorkTool.Core.Modules.ReactiveUI.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister dependencyInjectorRegister)
    {
        dependencyInjectorRegister.RegisterSingleton<IScheduler>(RxApp.MainThreadScheduler);
    }
}
