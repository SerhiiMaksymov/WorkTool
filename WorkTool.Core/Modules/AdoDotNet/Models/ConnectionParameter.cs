namespace WorkTool.Core.Modules.AdoDotNet.Models;

public record ConnectionParameter
{
    public ConnectionParameterInfo Info { get; }
    public ConnectionParameterValue ParameterValue { get; }

    public ConnectionParameter(
        ConnectionParameterInfo info,
        ConnectionParameterValue parameterValue
    )
    {
        Info = info;
        ParameterValue = parameterValue;
    }
}
