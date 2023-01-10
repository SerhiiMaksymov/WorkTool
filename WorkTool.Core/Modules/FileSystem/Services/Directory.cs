namespace WorkTool.Core.Modules.FileSystem.Services;

public class Directory : IDirectory
{
    private readonly IDirectoryService directoryService;

    public Directory(string path, IDirectoryService directoryService)
        : this(new DirectoryInfo(path), directoryService) { }

    public Directory(DirectoryInfo directory, IDirectoryService directoryService)
    {
        this.directoryService = directoryService;
        Name = directory.Name;

        if (directory.Parent is not null)
        {
            Path = directory.Parent.FullName;
        }
    }

    public string Name { get; }
    public FileSystemPath Path { get; }

    public IEnumerable<IFile> GetFiles()
    {
        return directoryService.GetFiles(this);
    }

    public IEnumerable<IDirectory> GetDirectories()
    {
        return directoryService.GetDirectories(this);
    }

    public static implicit operator DirectoryInfo(Directory directory)
    {
        return directory.ToDirectoryInfo();
    }
}
