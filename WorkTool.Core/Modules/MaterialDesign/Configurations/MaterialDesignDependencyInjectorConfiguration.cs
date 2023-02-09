using Material.Colors;
using Material.Styles.Themes.Base;

using WorkTool.Core.Modules.Configuration.Extensions;
using WorkTool.Core.Modules.MaterialDesign.Helpers;
using WorkTool.Core.Modules.MaterialDesign.Services;

namespace WorkTool.Core.Modules.MaterialDesign.Configurations;

public readonly struct MaterialDesignDependencyInjectorConfiguration
    : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient<MaterialTheme>(
            (IConfiguration configuration, Uri uri) =>
                new MaterialTheme(uri)
                {
                    BaseTheme = configuration.GetValue(
                        MaterialDesignConfigurationPaths.ConfigBaseThemePath,
                        value => value.ToEnum<BaseThemeMode>(),
                        BaseThemeMode.Dark
                    ),
                    PrimaryColor = configuration.GetValue(
                        MaterialDesignConfigurationPaths.ConfigPrimaryColorPath,
                        value => value.ToEnum<PrimaryColor>(),
                        PrimaryColor.Amber
                    ),
                    SecondaryColor = configuration.GetValue(
                        MaterialDesignConfigurationPaths.ConfigSecondaryColorPath,
                        value => value.ToEnum<SecondaryColor>(),
                        SecondaryColor.Blue
                    )
                }
        );

        register.RegisterTransient<MaterialIconsTheme>((Uri uri) => new MaterialIconsTheme(uri));
    }
}
