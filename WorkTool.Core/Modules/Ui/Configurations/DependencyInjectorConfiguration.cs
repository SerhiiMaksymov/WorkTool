namespace WorkTool.Core.Modules.Ui.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient<UiContext>(
            (UiContextBuilder uiContextBuilder) =>
                uiContextBuilder.AddFromAssembly(typeof(WorkToolCoreMarcType).Assembly).Build()
        );
    }
}
