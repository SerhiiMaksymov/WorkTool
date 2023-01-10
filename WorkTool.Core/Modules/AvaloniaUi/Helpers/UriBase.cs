namespace WorkTool.Core.Modules.AvaloniaUi.Helpers;

public static class UriBase
{
    public const string AppStyleString = "avares://App/Styles";

    public const string DataGridThemeFluentString =
        "avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml";

    public const string ControlsStylesString =
        "avares://WorkTool.Core/Modules/AvaloniaUi/Styles/Controls.axaml";

    public static readonly Uri AppStyleUri = new(AppStyleString);
    public static readonly Uri DataGridThemeFluentUri = new(DataGridThemeFluentString);
    public static readonly Uri ControlsStylesUri = new(ControlsStylesString);
}
