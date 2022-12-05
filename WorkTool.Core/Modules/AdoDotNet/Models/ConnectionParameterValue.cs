namespace WorkTool.Core.Modules.AdoDotNet.Models;

public abstract record ConnectionParameterValue
{
    public string Value { get; }

    public ConnectionParameterValue(string value)
    {
        Value = value;
    }
}
