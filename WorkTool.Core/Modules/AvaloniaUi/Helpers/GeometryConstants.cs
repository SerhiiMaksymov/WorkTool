namespace WorkTool.Core.Modules.AvaloniaUi.Helpers;

public static class GeometryConstants
{
    public static Geometry Play => Geometry.Parse(DataConstants.Play);
    public static Geometry Stop => Geometry.Parse(DataConstants.Stop);
    public static Geometry SkipNext => Geometry.Parse(DataConstants.SkipNext);
    public static Geometry Send => Geometry.Parse(DataConstants.Send);
    public static Geometry Close => Geometry.Parse(DataConstants.Close);
    public static Geometry Download => Geometry.Parse(DataConstants.Download);
    public static Geometry Refresh => Geometry.Parse(DataConstants.Refresh);
}
