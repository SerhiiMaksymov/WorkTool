namespace WorkTool.Core.Modules.CommandLine.Models;

public class UInt32CommandLineArgumentMeta : CommandLineArgumentMeta<Ref<uint>>
{
    public UInt32CommandLineArgumentMeta(string key, Ref<uint> value) : base(key, value) { }

    public override Ref<uint> Parse(string value)
    {
        return new Ref<uint>(uint.Parse(value));
    }
}
