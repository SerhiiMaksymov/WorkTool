namespace WorkTool.Core.Modules.AdoDotNet.Models;

public record Int32ConnectionParameterValue : ConnectionParameterValue
{
    public int Int32Value { get; }

    public Int32ConnectionParameterValue(int value) : base(value.ToString())
    {
        Int32Value = value;
    }
}