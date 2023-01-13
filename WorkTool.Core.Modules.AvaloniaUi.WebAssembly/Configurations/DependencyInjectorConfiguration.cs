namespace WorkTool.Core.Modules.AvaloniaUi.WebAssembly.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient<AvaloniaUiApplication, BrowserAvaloniaUiApplication>();

        register.RegisterTransient<AppBuilder>(
            (AvaloniaUiApp avaloniaUiApp) =>
                AppBuilder.Configure(() => avaloniaUiApp).UseReactiveUI()
        );
    }
}
