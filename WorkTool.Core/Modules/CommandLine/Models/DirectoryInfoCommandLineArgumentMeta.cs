namespace WorkTool.Core.Modules.CommandLine.Models;

public class DirectoryInfoCommandLineArgumentMeta : CommandLineArgumentMeta<DirectoryInfo>
{
    public DirectoryInfoCommandLineArgumentMeta(string key, DirectoryInfo directory) : base(key, directory)
    {
    }

    public override DirectoryInfo Parse(string value)
    {
        return value.ToDirectory();
    }
}