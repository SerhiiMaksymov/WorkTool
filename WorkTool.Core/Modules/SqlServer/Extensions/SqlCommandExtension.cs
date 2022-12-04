namespace WorkTool.Core.Modules.SqlServer.Extensions;

public static class SqlCommandExtension
{
    public static Task<int> ExecuteNonQueryAsync(this SqlCommand command, string query)
    {
        return command.ExecuteNonQueryAsync<SqlCommand>(query);
    }

    public static Task<int> ExecuteNonQueryAsync(this SqlCommand           command, string query,
                                                 IEnumerable<SqlParameter> parameters)
    {
        return command.ExecuteNonQueryAsync<SqlCommand, SqlParameter>(query, parameters);
    }

    public static int ExecuteNonQuery(this SqlCommand command, string query)
    {
        return command.ExecuteNonQuery<SqlCommand>(query);
    }

    public static Task<object> ExecuteScalarAsync(this SqlCommand command, string query)
    {
        return command.ExecuteScalarAsync<SqlCommand>(query);
    }

    public static Task<object> ExecuteScalarAsync(this SqlCommand           command,
                                                  string                    query,
                                                  IEnumerable<SqlParameter> parameters)
    {
        return command.ExecuteScalarAsync<SqlCommand, SqlParameter>(query, parameters);
    }

    public static Task<DataTable> GetDataTableAsync(this SqlCommand command, string query)
    {
        return command.GetDataTableAsync<SqlCommand>(query);
    }

    public static Task<DataTable> GetDataTableAsync(this SqlCommand           command,
                                                    string                    query,
                                                    IEnumerable<SqlParameter> parameters)
    {
        return command.GetDataTableAsync<SqlCommand, SqlParameter>(query, parameters);
    }

    public static DataTable GetDataTable(this SqlCommand command)
    {
        return command.GetDataTable<SqlCommand>();
    }

    public static DataTable GetDataTable(this SqlCommand command, string query)
    {
        return command.GetDataTable<SqlCommand>(query);
    }

    public static DataTable GetDataTable(this SqlCommand           command,
                                         string                    query,
                                         IEnumerable<SqlParameter> parameters)
    {
        return command.GetDataTable<SqlCommand, SqlParameter>(query, parameters);
    }
}