namespace WorkTool.Core.Modules.CommandLine.Models;

public class FileInfoCommandLineArgumentMeta : CommandLineArgumentMeta<FileInfo>
{
    public FileInfoCommandLineArgumentMeta(string key, FileInfo file) : base(key, file) { }

    public override FileInfo Parse(string value)
    {
        return value.ToFile();
    }
}
