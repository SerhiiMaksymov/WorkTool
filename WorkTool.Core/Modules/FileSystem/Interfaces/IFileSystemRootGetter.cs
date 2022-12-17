namespace WorkTool.Core.Modules.FileSystem.Interfaces;

public interface IFileSystemRootGetter
{
    Span<FileSystemDirectory> GetFileSystemRoot();
}
