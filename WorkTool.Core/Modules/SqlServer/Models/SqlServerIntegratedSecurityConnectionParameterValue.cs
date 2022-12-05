namespace WorkTool.Core.Modules.SqlServer.Models;

public record SqlServerIntegratedSecurityConnectionParameterValue : ConnectionParameterValue
{
    public SqlServerIntegratedSecurityConnectionParameterValue(SqlServerIntegratedSecurity value)
        : base(value.ToString()) { }
}
