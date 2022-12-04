namespace WorkTool.Core.Modules.CommandLine.Models;

public class BooleanCommandLineArgumentMeta : CommandLineArgumentMeta<Ref<bool>>
{
    public BooleanCommandLineArgumentMeta(string key, Ref<bool> value) : base(key, value)
    {
    }

    public override Ref<bool> Parse(string value)
    {
        var result = bool.Parse(value);

        return new Ref<bool>(result);
    }
}