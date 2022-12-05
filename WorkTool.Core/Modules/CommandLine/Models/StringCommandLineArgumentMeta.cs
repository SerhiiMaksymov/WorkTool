namespace WorkTool.Core.Modules.CommandLine.Models;

public class StringCommandLineArgumentMeta : CommandLineArgumentMeta<string>
{
    public StringCommandLineArgumentMeta(string key, string str) : base(key, str) { }

    public override string Parse(string value)
    {
        return value;
    }
}
