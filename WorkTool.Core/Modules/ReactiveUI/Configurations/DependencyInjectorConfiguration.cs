using WorkTool.Core.Modules.ReactiveUI.Services;

namespace WorkTool.Core.Modules.ReactiveUI.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterSingleton<IScheduler>(RxApp.MainThreadScheduler);
        register.RegisterTransient<
            INavigationService<object>,
            RoutedViewHostNavigationService<object>
        >();
    }
}
