namespace WorkTool.Core.Modules.AdoDotNet.Models;

public record BooleanConnectionParameterValue(bool BooleanValue)
    : ConnectionParameterValue(BooleanValue.ToString());
