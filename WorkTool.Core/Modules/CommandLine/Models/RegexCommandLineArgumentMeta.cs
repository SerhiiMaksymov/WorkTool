namespace WorkTool.Core.Modules.CommandLine.Models;

public class RegexCommandLineArgumentMeta : CommandLineArgumentMeta<Regex>
{
    public RegexOptions Options { get; }

    public RegexCommandLineArgumentMeta(string key, Regex @default, RegexOptions options) : base(key, @default)
    {
        Options = options;
    }

    public override Regex Parse(string value)
    {
        return new Regex(value);
    }
}