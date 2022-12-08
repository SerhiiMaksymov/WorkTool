namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class ControlThemeExtension
{
    public static TControlTheme SetBasedOn<TControlTheme>(
        this TControlTheme target,
        Type targetType
    ) where TControlTheme : ControlTheme
    {
        var avaloniaApplicationCurrent = AvaloniaApplication.Current.ThrowIfNull();
        var resource = avaloniaApplicationCurrent.FindResource(targetType);

        if (resource is ControlTheme controlTheme)
        {
            return target.SetBasedOn(controlTheme);
        }

        resource = resource.ThrowIfNull();
        throw new TypeInvalidCastException(typeof(ControlTheme), resource.GetType());
    }
}
