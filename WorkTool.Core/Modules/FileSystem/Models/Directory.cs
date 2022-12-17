namespace WorkTool.Core.Modules.FileSystem.Models;

public record Directory(
    FileSystemPath Path,
    ReadOnlyMemory<Directory> Directories,
    ReadOnlyMemory<FileSystemFile> Files,
    ulong Size
)
{
    public static implicit operator Directory(DirectoryInfo directoryInfo)
    {
        var fileInfos = directoryInfo.GetFiles();
        var directoryInfos = directoryInfo.GetDirectories();
        var files = new List<File>();
        var directories = new List<Directory>();
        var size = 0ul;

        foreach (var fileInfo in fileInfos)
        {
            try
            {
                if (!fileInfo.Exists)
                {
                    continue;
                }

                if (fileInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    continue;
                }

                var file = (File)fileInfo;
                files.Add(file);
                size += file.Size;
            }
            catch (UnauthorizedAccessException) { }
            catch (DirectoryNotFoundException) { }
        }

        foreach (var dirInfo in directoryInfos)
        {
            try
            {
                if (!dirInfo.Exists)
                {
                    continue;
                }

                if (dirInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    continue;
                }

                var directory = (Directory)dirInfo;
                directories.Add(directory);
                size += directory.Size;
            }
            catch (UnauthorizedAccessException) { }
            catch (DirectoryNotFoundException) { }
        }

        return new Directory(directoryInfo.FullName, directories.ToArray(), files.ToArray(), size);
    }
}
