namespace WorkTool.Core.Modules.AdoDotNet.Models;

public record Int32ConnectionParameterInfo : ConnectionParameterInfo
{
    public int DefaultInt32Value { get; }

    public Int32ConnectionParameterInfo(string defaultAlias, IEnumerable<string> aliases, int defaultValue)
        : base(defaultAlias, aliases, defaultValue.ToString())
    {
        DefaultInt32Value = defaultValue;
    }
}