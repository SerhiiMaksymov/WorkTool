namespace WorkTool.Core.Modules.AdoDotNet.Exceptions;

public class ConnectionParametersException<TConnectionParameters> : Exception
    where TConnectionParameters : IConnectionParameters
{
    public TConnectionParameters ConnectionParameters { get; }

    public ConnectionParametersException(TConnectionParameters connectionParameters, Exception inner)
        : base($"Exception in {connectionParameters.GetSafeConnectionString()}", inner)
    {
        ConnectionParameters = connectionParameters;
    }
}