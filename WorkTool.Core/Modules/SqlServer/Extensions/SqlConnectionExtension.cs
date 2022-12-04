namespace WorkTool.Core.Modules.SqlServer.Extensions;

public static class SqlConnectionExtension
{
    public static Task ExecuteAsync(this SqlConnection con, Func<SqlCommand, Task> func)
    {
        return con.ExecuteAsync<SqlConnection, SqlCommand>(func);
    }
}