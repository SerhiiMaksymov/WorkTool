namespace WorkTool.Core.Modules.Sqlite.Models;

public record SqliteConnectionParameters : ConnectionParametersBase
{
    public const string DefaultKeyDataSource = "Data Source";

    public const string DefaultValueDataSource = "";

    public static readonly StringConnectionParameterInfo DataSourceParameterInfo;

    static SqliteConnectionParameters()
    {
        DataSourceParameterInfo = new StringConnectionParameterInfo(
            DefaultKeyDataSource,
            new[] { DefaultKeyDataSource },
            DefaultValueDataSource
        );

        ConnectionParameterValues[typeof(SqliteConnectionParameters)] =
            new ConnectionParameterInfo[] { DataSourceParameterInfo };
    }

    public SqliteConnectionParameters()
    {
        DataSource = DefaultValueDataSource;
    }

    public override IEnumerable<ConnectionParameter> Parameters =>
        ParameterValues.Select(x => new ConnectionParameter(x.Key, x.Value));

    public override IEnumerable<ConnectionParameter> SafeParameters => Parameters;

    public string DataSource
    {
        get
        {
            if (ParameterValues.ContainsKey(DataSourceParameterInfo))
            {
                return ParameterValues[DataSourceParameterInfo].Value;
            }

            return DataSourceParameterInfo.DefaultValue;
        }
        init =>
            ParameterValues[DataSourceParameterInfo] = new StringConnectionParameterValue(value);
    }
}
