namespace WorkTool.Core.Modules.CommandLine.Models;

public class UInt16CommandLineArgumentMeta : CommandLineArgumentMeta<Ref<ushort>>
{
    public UInt16CommandLineArgumentMeta(string key, Ref<ushort> value) : base(key, value)
    {
    }

    public override Ref<ushort> Parse(string value)
    {
        return new Ref<ushort>(ushort.Parse(value));
    }
}