namespace WorkTool.Core.Modules.MaterialDesign.Configurations;

public readonly struct MaterialDesignDependencyInjectorConfiguration
    : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient<MaterialTheme>((Uri uri) => new MaterialTheme(uri));
        //register.RegisterTransientItem<IStyle>((Uri uri) => new MaterialIconsTheme(uri));
    }
}
