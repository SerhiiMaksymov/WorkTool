namespace WorkTool.Core.Modules.FileSystem.Interfaces;

public interface IDirectoryService
{
    IEnumerable<IFile> GetFiles(IDirectory directory);
    IEnumerable<IDirectory> GetDirectories(IDirectory directory);
}
