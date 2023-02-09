using WorkTool.Core.Modules.Sqlite.Models;

namespace WorkTool.Core.Modules.Sqlite.Extensions;

public static class StringExtension
{
    public static SqliteConnectionParameters ToSqliteConnectionParameters(this string str)
    {
        var result = new SqliteConnectionParameters();
        var values = str.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (var value in values)
        {
            var parameters = value.Split('=');
            result.Set(parameters[0], parameters[1]);
        }

        return result;
    }
}
