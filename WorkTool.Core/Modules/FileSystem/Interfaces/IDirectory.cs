namespace WorkTool.Core.Modules.FileSystem.Interfaces;

public interface IDirectory
{
    string Name { get; }
    FileSystemPath Path { get; }

    IEnumerable<IFile> GetFiles();
    IEnumerable<IDirectory> GetDirectories();
}
