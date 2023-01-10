namespace WorkTool.Core.Modules.FileSystem.Extensions;

public static class DirectoryExtension
{
    public static string ToPathString(this IDirectory directory)
    {
        if (directory.Name == "/")
        {
            return directory.Name;
        }

        var path = directory.Path.ToString();

        if (path.Length == 1 && path[0] == SystemPath.DirectorySeparatorChar)
        {
            return $"{path}{directory.Name}";
        }

        return $"{path}{SystemPath.DirectorySeparatorChar}{directory.Name}";
    }

    public static DirectoryInfo ToDirectoryInfo(this IDirectory directory)
    {
        var path = directory.ToPathString();

        return new DirectoryInfo(path);
    }
}
