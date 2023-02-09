namespace WorkTool.Core.Modules.AdoDotNet.Models;

public record Int32ConnectionParameterValue(int Int32Value)
    : ConnectionParameterValue(Int32Value.ToString());
