namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class LayoutableExtension
{
    public static TLayoutable SetMargin<TLayoutable>(
        this TLayoutable layoutable,
        double left,
        double top,
        double right,
        double bottom
    ) where TLayoutable : Layoutable
    {
        var thickness = new Thickness(left, top, right, bottom);

        return layoutable.SetMargin(thickness);
    }

    public static TLayoutable SetMargin<TLayoutable>(this TLayoutable layoutable, double margin)
        where TLayoutable : Layoutable
    {
        var thickness = new Thickness(margin);

        return layoutable.SetMargin(thickness);
    }
}
