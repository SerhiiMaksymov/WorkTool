namespace WorkTool.Core.Modules.AdoDotNet.Extensions;

public static class ConnectionParametersExtension
{
    public static IEnumerable<ConnectionParameter> GetNoDefaultParameters<TConnectionParameters>(
        this TConnectionParameters connectionItems
    ) where TConnectionParameters : IConnectionParameters
    {
        var result = connectionItems.Parameters.GetNoDefaultParameters();

        return result;
    }

    public static string GetConnectionString<TConnectionParameters>(
        this TConnectionParameters connectionItems
    ) where TConnectionParameters : IConnectionParameters
    {
        var noDefaultParameters = connectionItems.GetNoDefaultParameters();
        var result = noDefaultParameters.GetConnectionString();

        return result;
    }

    public static string GetSafeConnectionString<TConnectionParameters>(
        this TConnectionParameters connectionItems
    ) where TConnectionParameters : IConnectionParameters
    {
        return connectionItems.SafeParameters.GetNoDefaultParameters().GetConnectionString();
    }

    public static TDbConnection ToDbConnection<TConnectionParameters, TDbConnection>(
        this TConnectionParameters connection
    )
        where TConnectionParameters : IConnectionParameters
        where TDbConnection : DbConnection, new()
    {
        var connectionString = connection.GetConnectionString();

        return new TDbConnection { ConnectionString = connectionString };
    }

    public static Task ExecuteAsync<TConnectionParameters, TDbConnection, TCommand>(
        this TConnectionParameters connection,
        Func<TCommand, Task> func
    )
        where TConnectionParameters : IConnectionParameters
        where TDbConnection : DbConnection, new()
        where TCommand : DbCommand, new()
    {
        var dDConnection = connection.ToDbConnection<TConnectionParameters, TDbConnection>();

        return dDConnection.ExecuteAsync(func);
    }

    public static async Task<string> GetCsvStringAsync<
        TConnectionParameters,
        TDbConnection,
        TCommand
    >(this TConnectionParameters connection, string query)
        where TConnectionParameters : IConnectionParameters
        where TDbConnection : DbConnection, new()
        where TCommand : DbCommand, new()
    {
        await using var dDConnection = connection.ToDbConnection<
            TConnectionParameters,
            TDbConnection
        >();
        var result = await dDConnection.GetCsvStringAsync<TCommand, TDbConnection>(query);

        return result;
    }

    public static async Task<string> GetTableStringAsync<
        TConnectionParameters,
        TDbConnection,
        TCommand
    >(this TConnectionParameters connection, string query, string rowSeparator, int padding)
        where TConnectionParameters : IConnectionParameters
        where TDbConnection : DbConnection, new()
        where TCommand : DbCommand, new()
    {
        await using var dDConnection = connection.ToDbConnection<
            TConnectionParameters,
            TDbConnection
        >();
        var result = await dDConnection.GetTableStringAsync<TCommand, TDbConnection>(
            query,
            rowSeparator,
            padding
        );

        return result;
    }

    public static async Task<DataTable> GetDataTableAsync<
        TConnectionParameters,
        TDbConnection,
        TCommand
    >(this TConnectionParameters connection, string query)
        where TConnectionParameters : IConnectionParameters
        where TDbConnection : DbConnection, new()
        where TCommand : DbCommand, new()
    {
        await using var dDConnection = connection.ToDbConnection<
            TConnectionParameters,
            TDbConnection
        >();

        return await dDConnection.GetDataTableAsync<TCommand, TDbConnection>(query);
    }

    public static async Task<DataTable> GetDataTableAsync<
        TConnectionParameters,
        TDbConnection,
        TCommand,
        TDataAdapter
    >(this TConnectionParameters connection, string query)
        where TConnectionParameters : IConnectionParameters
        where TDbConnection : DbConnection, new()
        where TCommand : DbCommand, new()
        where TDataAdapter : DbDataAdapter, new()
    {
        await using var dDConnection = connection.ToDbConnection<
            TConnectionParameters,
            TDbConnection
        >();

        return await dDConnection.GetDataTableAsync<TCommand, TDbConnection, TDataAdapter>(query);
    }

    public static DataTable GetDataTable<
        TConnectionParameters,
        TDbConnection,
        TCommand,
        TDataAdapter
    >(this TConnectionParameters connection, string query)
        where TConnectionParameters : IConnectionParameters
        where TDbConnection : DbConnection, new()
        where TCommand : DbCommand, new()
        where TDataAdapter : DbDataAdapter, new()
    {
        using var dDConnection = connection.ToDbConnection<TConnectionParameters, TDbConnection>();

        return dDConnection.GetDataTable<TCommand, TDbConnection, TDataAdapter>(query);
    }

    public static async Task<int> ExecuteNonQueryAsync<
        TConnectionParameters,
        TDbConnection,
        TCommand
    >(this TConnectionParameters connection, string query)
        where TConnectionParameters : IConnectionParameters
        where TDbConnection : DbConnection, new()
        where TCommand : DbCommand, new()
    {
        await using var dDConnection = connection.ToDbConnection<
            TConnectionParameters,
            TDbConnection
        >();

        return await dDConnection.ExecuteNonQueryAsync<TCommand, TDbConnection>(query);
    }

    public static async Task<object?> ExecuteScalarAsync<
        TConnectionParameters,
        TDbConnection,
        TCommand
    >(this TConnectionParameters connection, string query)
        where TConnectionParameters : IConnectionParameters
        where TDbConnection : DbConnection, new()
        where TCommand : DbCommand, new()
    {
        await using var dDConnection = connection.ToDbConnection<
            TConnectionParameters,
            TDbConnection
        >();

        return await dDConnection.ExecuteScalarAsync<TCommand, TDbConnection>(query);
    }

    public static async Task<object?> ExecuteScalarAsync<
        TConnectionParameters,
        TDbConnection,
        TCommand,
        TParameter
    >(this TConnectionParameters connection, string query, IEnumerable<TParameter> parameters)
        where TConnectionParameters : IConnectionParameters
        where TDbConnection : DbConnection, new()
        where TCommand : DbCommand, new()
        where TParameter : DbParameter
    {
        await using var dDConnection = connection.ToDbConnection<
            TConnectionParameters,
            TDbConnection
        >();

        return await dDConnection.ExecuteScalarAsync<TCommand, TDbConnection, TParameter>(
            query,
            parameters
        );
    }

    public static async Task<int> ExecuteNonQueryAsync<
        TConnectionParameters,
        TDbConnection,
        TCommand,
        TParameter
    >(this TConnectionParameters connection, string query, IEnumerable<TParameter> parameters)
        where TConnectionParameters : IConnectionParameters
        where TDbConnection : DbConnection, new()
        where TCommand : DbCommand, new()
        where TParameter : DbParameter
    {
        await using var dDConnection = connection.ToDbConnection<
            TConnectionParameters,
            TDbConnection
        >();

        return await dDConnection.ExecuteNonQueryAsync<TCommand, TDbConnection, TParameter>(
            query,
            parameters
        );
    }

    public static DataTable GetDataTable<TConnectionParameters, TDbConnection, TCommand>(
        this TConnectionParameters connection,
        string query
    )
        where TConnectionParameters : IConnectionParameters
        where TDbConnection : DbConnection, new()
        where TCommand : DbCommand, new()
    {
        using var dDConnection = connection.ToDbConnection<TConnectionParameters, TDbConnection>();

        return dDConnection.GetDataTable<TCommand, TDbConnection>(query);
    }
}
