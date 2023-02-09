namespace WorkTool.Core.Modules.Ui.Configurations;

public readonly struct UiDependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient<UiContext>(
            (UiContextBuilder uiContextBuilder) => uiContextBuilder.AddFromAssemblies().Build()
        );
    }
}
