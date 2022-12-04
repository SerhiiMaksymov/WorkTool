namespace WorkTool.Core.Modules.SqlServer.Extensions;

public static class StringExtension
{
    public static SqlServerConnectionParameters ToSqlServerConnectionParameters(this string str)
    {
        var result = new SqlServerConnectionParameters();
        var values = str.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (var value in values)
        {
            var parameters = value.Split('=');
            result.Set(parameters[0], parameters[1]);
        }

        return result;
    }
}