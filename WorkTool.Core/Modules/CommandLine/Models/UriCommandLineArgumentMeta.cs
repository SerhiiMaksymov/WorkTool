namespace WorkTool.Core.Modules.CommandLine.Models;

public class UriCommandLineArgumentMeta : CommandLineArgumentMeta<Uri>
{
    public UriCommandLineArgumentMeta(string key, Uri @default) : base(key, @default) { }

    public override Uri Parse(string value)
    {
        return new Uri(value);
    }
}
