namespace WorkTool.Core.Modules.CommandLine.Models;

public class ArrayStringCommandLineArgumentMeta : CommandLineArgumentMeta<string[]>
{
    public string Separator { get; }
    public StringSplitOptions SplitOptions { get; }

    public ArrayStringCommandLineArgumentMeta(
        string key,
        string[] @default,
        string separator,
        StringSplitOptions splitOptions
    ) : base(key, @default)
    {
        Separator = separator.ThrowIfNullOrWhiteSpace(nameof(separator));
        SplitOptions = splitOptions;
    }

    public override string[] Parse(string value)
    {
        return value.Split(Separator, SplitOptions);
    }
}
