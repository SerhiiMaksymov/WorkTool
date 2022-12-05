namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class ColorExtension
{
    public static AvaloniaColor ToAvalonia(this SystemColor color)
    {
        return new Color(color.A, color.R, color.G, color.B);
    }

    public static IBrush ToBrush(this AvaloniaColor color)
    {
        return new SolidColorBrush(color);
    }
}
