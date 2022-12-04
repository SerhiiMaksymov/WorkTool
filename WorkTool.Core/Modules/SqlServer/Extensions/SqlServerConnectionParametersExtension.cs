namespace WorkTool.Core.Modules.SqlServer.Extensions;

public static class SqlServerConnectionParametersExtension
{
    public static SqlConnection ToDbConnection(this SqlServerConnectionParameters connection)
    {
        var connectionString = connection.GetConnectionString();

        return connection.ToDbConnection<SqlServerConnectionParameters, SqlConnection>();
    }

    public static async Task<string> GetTableStringAsync(this SqlServerConnectionParameters connection, string query)
    {
        await using var dDConnection = connection.ToDbConnection();

        var result = await dDConnection.GetTableStringAsync<SqlCommand, SqlConnection>(
            query,
            AdoDotNetConstants.DefaultRowSeparator,
            AdoDotNetConstants.DefaultPadding);

        return result;
    }

    public static async Task<int> ExecuteNonQueryAsync(this SqlServerConnectionParameters connection, string query)
    {
        await using var dDConnection = connection.ToDbConnection();

        return await dDConnection.ExecuteNonQueryAsync<SqlCommand, SqlConnection>(query);
    }

    public static async Task<object> ExecuteScalarAsync(this SqlServerConnectionParameters connection, string query)
    {
        await using var dDConnection = connection.ToDbConnection();

        return await dDConnection.ExecuteScalarAsync<SqlCommand, SqlConnection>(query);
    }

    public static async Task<object> ExecuteScalarAsync(this SqlServerConnectionParameters connection, string query,
                                                        IEnumerable<SqlParameter>          parameters)
    {
        await using var dDConnection = connection.ToDbConnection();

        return await dDConnection.ExecuteScalarAsync<SqlCommand, SqlConnection, SqlParameter>(query, parameters);
    }

    public static async Task<int> ExecuteNonQueryAsync(this SqlServerConnectionParameters connection, string query,
                                                       IEnumerable<SqlParameter>          parameters)
    {
        await using var dDConnection = connection.ToDbConnection();

        return await dDConnection.ExecuteNonQueryAsync<SqlCommand, SqlConnection, SqlParameter>(query, parameters);
    }

    public static DataTable GetDataTable(this SqlServerConnectionParameters connection, string query)
    {
        using var dDConnection = connection.ToDbConnection();

        return dDConnection.GetDataTable<SqlCommand, SqlConnection, SqlDataAdapter>(query);
    }

    public static async Task<DataTable> GetDataTableAsync(this SqlServerConnectionParameters connection, string query)
    {
        await using var dDConnection = connection.ToDbConnection();

        return await dDConnection.GetDataTableAsync<SqlCommand, SqlConnection>(query);
    }

    public static Task ExecuteAsync(this SqlServerConnectionParameters connection, Func<SqlCommand, Task> func)
    {
        var dDConnection = connection.ToDbConnection();

        return dDConnection.ExecuteAsync<SqlConnection, SqlCommand>(func);
    }

    public static DbContextOptions<TDbContext> ToDbContextOptions<TDbContext>(
    this SqlServerConnectionParameters connection)
        where TDbContext : DbContext
    {
        var result = new DbContextOptionsBuilder<TDbContext>();
        result.UseSqlServer(connection.GetConnectionString());

        return result.Options;
    }
}