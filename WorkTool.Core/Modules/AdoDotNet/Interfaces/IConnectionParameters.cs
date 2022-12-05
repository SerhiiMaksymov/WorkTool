namespace WorkTool.Core.Modules.AdoDotNet.Interfaces;

public interface IConnectionParameters
{
    IEnumerable<ConnectionParameter> Parameters { get; }
    IEnumerable<ConnectionParameter> SafeParameters { get; }
}
