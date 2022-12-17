namespace WorkTool.Core.Modules.FileSystem.Services;

public readonly struct FileSystemRootGetter : IFileSystemRootGetter
{
    public Span<FileSystemDirectory> GetFileSystemRoot()
    {
        var drives = DriveInfo.GetDrives();
        var roots = new FileSystemDirectory[drives.Length];

        for (var index = 0; index < drives.Length; index++)
        {
            roots[index] = drives[index].RootDirectory;
        }

        return roots;
    }
}
