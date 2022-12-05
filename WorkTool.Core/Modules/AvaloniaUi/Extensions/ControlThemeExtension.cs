namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class ControlThemeExtension
{
    public static TControlTheme SetBasedOn<TControlTheme>(
        this TControlTheme target,
        Type targetType
    ) where TControlTheme : ControlTheme
    {
        var resource = (ControlTheme)AvaloniaApplication.Current.FindResource(targetType);

        return target.SetBasedOn(resource);
    }
}
