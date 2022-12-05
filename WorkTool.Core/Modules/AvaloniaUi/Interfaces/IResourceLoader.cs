namespace WorkTool.Core.Modules.AvaloniaUi.Interfaces;

public interface IResourceLoader
{
    IEnumerable<IResourceProvider> LoadResources();
}
