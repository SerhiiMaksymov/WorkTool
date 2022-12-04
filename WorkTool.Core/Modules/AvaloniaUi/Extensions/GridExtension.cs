namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class GridExtension
{
    public static TGrid AddColumnDefinition<TGrid>(this TGrid grid, GridLength length) where TGrid : Grid
    {
        grid.AddColumnDefinition(new ColumnDefinition(length));

        return grid;
    }

    public static TGrid AddColumnDefinitions<TGrid>(this TGrid grid, params GridLength[] lengths) where TGrid : Grid
    {
        foreach (var length in lengths)
        {
            grid.AddColumnDefinition(length);
        }

        return grid;
    }

    public static TGrid AddColumnDefinition<TGrid>(this TGrid grid, double value, GridUnitType type) where TGrid : Grid
    {
        grid.AddColumnDefinition(new ColumnDefinition(value, type));

        return grid;
    }

    public static TGrid AddRowDefinition<TGrid>(this TGrid grid, GridLength length) where TGrid : Grid
    {
        grid.AddRowDefinition(new RowDefinition(length));

        return grid;
    }

    public static TGrid AddRowDefinitions<TGrid>(this TGrid grid, params GridLength[] lengths) where TGrid : Grid
    {
        foreach (var length in lengths)
        {
            grid.AddRowDefinition(length);
        }

        return grid;
    }

    public static TGrid AddRowDefinition<TGrid>(this TGrid grid, double value, GridUnitType type) where TGrid : Grid
    {
        grid.AddRowDefinition(new RowDefinition(value, type));

        return grid;
    }
}