namespace WorkTool.Core.Modules.AdoDotNet.Models;

public record ByteConnectionParameterValue(byte ByteVvalue)
    : ConnectionParameterValue(ByteVvalue.ToString());
