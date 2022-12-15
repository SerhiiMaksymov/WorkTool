namespace WorkTool.Core.Modules.AvaloniaUi.Helpers;

public static class UriBase
{
    public const string DataGridThemeFluentString =
        "avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml";

    public const string ControlsStylesString =
        "avares://WorkTool.Core/Modules/AvaloniaUi/Styles/Controls.axaml";

    public const string MaterialIconsString = "avares://Material.Icons.Avalonia/App.xaml";

    public static readonly Uri DataGridThemeFluentUri = new(DataGridThemeFluentString);
    public static readonly Uri MaterialIconsUri = new(MaterialIconsString);
    public static readonly Uri ControlsStylesUri = new(ControlsStylesString);
}
