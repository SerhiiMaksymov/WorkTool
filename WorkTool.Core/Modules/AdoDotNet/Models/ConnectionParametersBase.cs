namespace WorkTool.Core.Modules.AdoDotNet.Models;

public abstract record ConnectionParametersBase : IConnectionParameters
{
    protected static readonly Dictionary<
        Type,
        IEnumerable<ConnectionParameterInfo>
    > ConnectionParameterValues;

    public static readonly IReadOnlyDictionary<
        Type,
        IEnumerable<ConnectionParameterInfo>
    > ConnectionParameters;

    protected readonly Dictionary<
        ConnectionParameterInfo,
        ConnectionParameterValue
    > ParameterValues = new();

    static ConnectionParametersBase()
    {
        ConnectionParameterValues = new();
        ConnectionParameters = ConnectionParameterValues;
    }

    public abstract IEnumerable<ConnectionParameter> Parameters { get; }
    public abstract IEnumerable<ConnectionParameter> SafeParameters { get; }

    public void Set(string key, string value)
    {
        var connectionParameterInfo = ConnectionParameters[GetType()].SingleOrDefault(
            x => x.Aliases.Contains(key)
        );

        if (connectionParameterInfo is null)
        {
            throw new Exception($"Unknow key {key}.");
        }

        ParameterValues[connectionParameterInfo] = ParseValue(connectionParameterInfo, value);
    }

    private ConnectionParameterValue ParseValue(
        ConnectionParameterInfo connectionParameterInfo,
        string value
    )
    {
        switch (connectionParameterInfo)
        {
            case StringConnectionParameterInfo _:
            {
                return new StringConnectionParameterValue(value);
            }
            case BooleanConnectionParameterInfo _:
            {
                return new BooleanConnectionParameterValue(bool.Parse(value));
            }
            case SqlServerIntegratedSecurityConnectionParameterInfo _:
            {
                return new SqlServerIntegratedSecurityConnectionParameterValue(
                    Enum.Parse<SqlServerIntegratedSecurity>(value)
                );
            }
            case Int32ConnectionParameterInfo _:
            {
                return new Int32ConnectionParameterValue(int.Parse(value));
            }

            case ByteConnectionParameterInfo _:
            {
                return new ByteConnectionParameterValue(byte.Parse(value));
            }
            default:
            {
                throw new Exception($"{connectionParameterInfo.GetType()}");
            }
        }
    }
}
