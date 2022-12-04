namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class StyleLoader : IStyleLoader
{
    public IEnumerable<IStyle> LoadStyles()
    {
        return StyleLoaderHelper.LoaStylesFromAssemblies();
    }
}