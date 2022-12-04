namespace WorkTool.Core.Modules.SqlServer.Models;

public record SqlServerIntegratedSecurityConnectionParameterInfo : ConnectionParameterInfo
{
    public SqlServerIntegratedSecurityConnectionParameterInfo(string defaultAlias, IEnumerable<string> aliases,
                                                              SqlServerIntegratedSecurity defaultValue)
        : base(defaultAlias, aliases, defaultValue.ToString())
    {
    }
}