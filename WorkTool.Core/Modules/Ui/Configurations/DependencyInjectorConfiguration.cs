namespace WorkTool.Core.Modules.Ui.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister dependencyInjectorRegister)
    {
        dependencyInjectorRegister.RegisterTransient<UiContext>(
            (UiContextBuilder uiContextBuilder) =>
                uiContextBuilder.AddFromAssembly(typeof(WorkToolCoreMarcType).Assembly).Build()
        );
    }
}
