namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class StyleExtension
{
    public static TStyle AddSetter<TStyle>(
        this TStyle style,
        AvaloniaProperty property,
        object value
    ) where TStyle : Style
    {
        var setter = new Setter(property, value);

        return style.AddSetter(setter);
    }

    public static TStyle AddSetterDynamicResource<TStyle>(
        this TStyle style,
        AvaloniaProperty property,
        object resourceKey
    ) where TStyle : Style
    {
        var dynamicResourceBinding = new DynamicResourceBinding(resourceKey);

        return style.AddSetter(property, dynamicResourceBinding);
    }
}
