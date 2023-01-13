namespace WorkTool.Core.Modules.FileSystem.Services;

public class File : IFile
{
    public File(FileInfo file, IDirectoryService directoryService)
    {
        Size = (ulong)file.Length;
        FileName = file;

        if (file.Directory is not null)
        {
            Directory = new Directory(file.Directory, directoryService);
        }
    }

    public QuantitiesInformation Size { get; }
    public IDirectory? Directory { get; }
    public FileName FileName { get; }
}
