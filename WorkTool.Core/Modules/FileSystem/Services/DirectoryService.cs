namespace WorkTool.Core.Modules.FileSystem.Services;

public class DirectoryService : IDirectoryService
{
    public IEnumerable<IFile> GetFiles(IDirectory directory)
    {
        try
        {
            var dir = directory.ToDirectoryInfo();

            var files = dir.GetFiles()
                .Where(x => !x.Attributes.HasFlag(FileAttributes.ReparsePoint))
                .Select(x => new File(x, this));

            return files;
        }
        catch (UnauthorizedAccessException)
        {
            return Enumerable.Empty<IFile>();
        }
        catch (DirectoryNotFoundException)
        {
            return Enumerable.Empty<IFile>();
        }
    }

    public IEnumerable<IDirectory> GetDirectories(IDirectory directory)
    {
        try
        {
            var dir = directory.ToDirectoryInfo();

            var directories = dir.GetDirectories()
                .Where(x => !x.Attributes.HasFlag(FileAttributes.ReparsePoint))
                .OrderBy(x => x.Name)
                .Select(x => new Directory(x, this));

            return directories;
        }
        catch (UnauthorizedAccessException)
        {
            return Enumerable.Empty<IDirectory>();
        }
        catch (DirectoryNotFoundException)
        {
            return Enumerable.Empty<IDirectory>();
        }
    }
}
