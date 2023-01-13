using WorkTool.Core.Modules.MaterialDesign.Services;

namespace WorkTool.Core.Modules.MaterialDesign.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransientItem<IStyle>((Uri uri) => new MaterialTheme(uri));
        register.RegisterTransientItem<IStyle>((Uri uri) => new MaterialIconsTheme(uri));
    }
}
