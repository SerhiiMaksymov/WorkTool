namespace WorkTool.Core.Modules.FileSystem.Extensions;

public static class FileExtension
{
    public static string ToPathString(this IFile file)
    {
        if (file.Directory is null)
        {
            return file.FileName.ToString();
        }
        
        return $"{file.Directory.ToPathString()}{SystemPath.DirectorySeparatorChar}{file.FileName}";
    }
}