namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class ResourceLoader : IResourceLoader
{
    public IEnumerable<IResourceProvider> LoadResources()
    {
        return ResourceLoaderHelper.LoaStylesFromAssemblies();
    }
}