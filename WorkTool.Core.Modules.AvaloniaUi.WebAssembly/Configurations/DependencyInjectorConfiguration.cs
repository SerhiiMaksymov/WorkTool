namespace WorkTool.Core.Modules.AvaloniaUi.WebAssembly.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister dependencyInjectorRegister)
    {
        dependencyInjectorRegister.RegisterTransient<
            AvaloniaUiApplication,
            BrowserAvaloniaUiApplication
        >();

        dependencyInjectorRegister.RegisterTransient<AppBuilder>(
            (AvaloniaUiApp avaloniaUiApp) =>
                AppBuilder.Configure(() => avaloniaUiApp).UseReactiveUI()
        );
    }
}
