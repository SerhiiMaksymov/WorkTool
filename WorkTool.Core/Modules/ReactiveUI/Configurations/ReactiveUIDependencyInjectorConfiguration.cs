namespace WorkTool.Core.Modules.ReactiveUI.Configurations;

public readonly struct ReactiveUIDependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterSingleton(RxApp.MainThreadScheduler);
        register.RegisterTransient<
            INavigationService<object>,
            RoutedViewHostNavigationService<object>
        >();
    }
}
