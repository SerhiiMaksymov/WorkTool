namespace WorkTool.Core.Modules.SqlServer.Models;

public class
    SqlServerConnectionParametersCommandLineArgumentMeta : CommandLineArgumentMeta<SqlServerConnectionParameters>
{
    public SqlServerConnectionParametersCommandLineArgumentMeta(string key,
                                                                SqlServerConnectionParameters
                                                                    sqlServerConnectionParameters)
        : base(key, sqlServerConnectionParameters)
    {
    }

    public override SqlServerConnectionParameters Parse(string value)
    {
        return value.ToSqlServerConnectionParameters();
    }
}