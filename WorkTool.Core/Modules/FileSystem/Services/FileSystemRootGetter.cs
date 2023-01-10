namespace WorkTool.Core.Modules.FileSystem.Services;

public class FileSystemRootGetter : IFileSystemRootGetter
{
    private readonly IDirectoryService directoryService;

    public FileSystemRootGetter(IDirectoryService directoryService)
    {
        this.directoryService = directoryService;
    }

    public Span<IDirectory> GetFileSystemRoot()
    {
        var drives = DriveInfo.GetDrives();
        var roots = new FileSystemDirectory[drives.Length];

        for (var index = 0; index < drives.Length; index++)
        {
            roots[index] = new FileSystemDirectory(drives[index].RootDirectory, directoryService);
        }

        return roots;
    }
}
