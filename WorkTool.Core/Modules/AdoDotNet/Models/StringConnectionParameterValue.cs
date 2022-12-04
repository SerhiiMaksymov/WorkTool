namespace WorkTool.Core.Modules.AdoDotNet.Models;

public record StringConnectionParameterValue : ConnectionParameterValue
{
    public StringConnectionParameterValue(string value) : base(value)
    {
    }
}