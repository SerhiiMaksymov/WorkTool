namespace WorkTool.Core.Modules.AdoDotNet.Models;

public record BooleanConnectionParameterValue : ConnectionParameterValue
{
    public bool BooleanValue { get; }

    public BooleanConnectionParameterValue(bool value) : base(value.ToString())
    {
        BooleanValue = value;
    }
}
