namespace WorkTool.Core.Modules.AvaloniaUi.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister dependencyInjectorRegister)
    {
        dependencyInjectorRegister.RegisterTransient<
            AvaloniaUiApplication,
            DesktopAvaloniaUiApplication
        >();

        dependencyInjectorRegister.RegisterTransient<AppBuilder>(
            (AvaloniaUiApp avaloniaUiApp) =>
                AppBuilder.Configure(() => avaloniaUiApp).UseReactiveUI().UsePlatformDetect()
        );
    }
}
