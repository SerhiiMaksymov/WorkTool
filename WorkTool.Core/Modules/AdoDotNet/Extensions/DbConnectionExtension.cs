namespace WorkTool.Core.Modules.AdoDotNet.Extensions;

public static class DbConnectionExtension
{
    public static Task<TCommand> InitCommandAsync<TCommand, TConnection>(
        this TConnection connection
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        var timeout = AdoDotNetConstants.DefaultTimeout;

        return connection.InitCommandAsync<TCommand, TConnection>(string.Empty, timeout);
    }

    public static TCommand InitCommand<TCommand, TConnection>(this TConnection connection)
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        return connection.InitCommand<TCommand, TConnection>(
            string.Empty,
            AdoDotNetConstants.DefaultTimeout
        );
    }

    public static TCommand InitCommand<TCommand, TConnection>(
        this TConnection connection,
        string query
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        var timeout = AdoDotNetConstants.DefaultTimeout;

        return connection.InitCommand<TCommand, TConnection>(query, timeout);
    }

    public static Task<TCommand> InitCommandAsync<TCommand, TConnection>(
        this TConnection connection,
        string query
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        var timeout = AdoDotNetConstants.DefaultTimeout;

        return connection.InitCommandAsync<TCommand, TConnection>(query, timeout);
    }

    public static Task<TCommand> InitCommandAsync<TCommand, TConnection>(
        this TConnection connection,
        uint timeout
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        return connection.InitCommandAsync<TCommand, TConnection>(string.Empty, timeout);
    }

    public static async Task<TCommand> InitCommandAsync<TCommand, TConnection>(
        this TConnection connection,
        string query,
        uint timeout
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        query.ThrowIfNull(nameof(query));
        connection.ThrowIfNull(nameof(connection));
        await connection.OpenAsync();
        var transaction = await connection.BeginTransactionAsync();

        var result = new TCommand
        {
            CommandText = query,
            Connection = connection,
            Transaction = transaction,
            CommandTimeout = (int)timeout
        };

        return result;
    }

    public static TCommand InitCommand<TCommand, TConnection>(
        this TConnection connection,
        string query,
        uint timeout
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        query.ThrowIfNull(nameof(query));
        connection.ThrowIfNull(nameof(connection));
        connection.Open();
        var transaction = connection.BeginTransaction();

        var result = new TCommand
        {
            CommandText = query,
            Connection = connection,
            Transaction = transaction,
            CommandTimeout = (int)timeout
        };

        return result;
    }

    public static async Task<int> ExecuteNonQueryAsync<TCommand, TConnection>(
        this TConnection con,
        string query
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        await using var command = await con.InitCommandAsync<TCommand, TConnection>(query);
        await using var transaction = command.Transaction.ThrowIfNull();
        await using var connection = command.Connection.ThrowIfNull();

        try
        {
            var result = await command.ExecuteNonQueryAsync();
            await transaction.CommitAsync();

            return result;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();

            throw new Exception(connection.ConnectionString, exception);
        }
    }

    public static async Task<object?> ExecuteScalarAsync<TCommand, TConnection>(
        this TConnection con,
        string query
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        await using var command = await con.InitCommandAsync<TCommand, TConnection>(query);
        await using var transaction = command.Transaction.ThrowIfNull();
        await using var connection = command.Connection.ThrowIfNull();

        try
        {
            var result = await command.ExecuteScalarAsync();
            await transaction.CommitAsync();

            return result;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();

            throw new Exception(connection.ConnectionString, exception);
        }
    }

    public static async Task ExecuteAsync<TConnection, TCommand>(
        this TConnection con,
        Func<TCommand, Task> func
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        await using var command = await con.InitCommandAsync<TCommand, TConnection>(string.Empty);
        await using var transaction = command.Transaction.ThrowIfNull();
        await using var connection = command.Connection.ThrowIfNull();

        try
        {
            await func.Invoke(command);
            await transaction.CommitAsync();
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();

            throw new Exception(connection.ConnectionString, exception);
        }
    }

    public static async Task<object?> ExecuteScalarAsync<TCommand, TConnection, TParameter>(
        this TConnection con,
        string query,
        IEnumerable<TParameter> parameters
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
        where TParameter : DbParameter
    {
        await using var command = await con.InitCommandAsync<TCommand, TConnection>(query);
        await using var transaction = command.Transaction.ThrowIfNull();
        await using var connection = command.Connection.ThrowIfNull();
        command.Parameters.AddRange(parameters.ToArray());

        try
        {
            var result = await command.ExecuteScalarAsync();
            await transaction.CommitAsync();

            return result;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();

            throw new Exception(connection.ConnectionString, exception);
        }
    }

    public static async Task<int> ExecuteNonQueryAsync<TCommand, TConnection, TParameter>(
        this TConnection con,
        string query,
        IEnumerable<TParameter> parameters
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
        where TParameter : DbParameter
    {
        await using var command = await con.InitCommandAsync<TCommand, TConnection>(query);
        await using var transaction = command.Transaction.ThrowIfNull();
        await using var connection = command.Connection.ThrowIfNull();
        command.Parameters.AddRange(parameters.ToArray());

        try
        {
            var result = await command.ExecuteNonQueryAsync();
            await transaction.CommitAsync();

            return result;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();

            throw new Exception(connection.ConnectionString, exception);
        }
    }

    public static async Task<string> GetCsvStringAsync<TCommand, TConnection>(
        this TConnection con,
        string query
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        await using var command = await con.InitCommandAsync<TCommand, TConnection>(query);
        await using var transaction = command.Transaction;
        await using var connection = command.Connection;
        await using var reader = await command.ExecuteReaderAsync();
        var result = await reader.GetCsvStringAsync(";", Environment.NewLine);

        return result;
    }

    public static async Task<string> GetTableStringAsync<TCommand, TConnection>(
        this TConnection con,
        string query,
        string rowSeparator,
        int padding
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        await using var command = await con.InitCommandAsync<TCommand, TConnection>(query);
        await using var transaction = command.Transaction;
        await using var connection = command.Connection;
        await using var reader = await command.ExecuteReaderAsync();
        var result = await reader.GetTableStringAsync(rowSeparator, padding);

        return result;
    }

    public static async Task<DataTable> GetDataTableAsync<TCommand, TConnection>(
        this TConnection con,
        string query
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        await using var command = await con.InitCommandAsync<TCommand, TConnection>();
        await using var transaction = command.Transaction;
        await using var connection = command.Connection;
        var result = await command.GetDataTableAsync(query);

        return result;
    }

    public static async Task<DataTable> GetDataTableAsync<TCommand, TConnection, TDataAdapter>(
        this TConnection con,
        string query
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
        where TDataAdapter : DbDataAdapter, new()
    {
        using var command = await con.InitCommandAsync<TCommand, TConnection>();
        using var transaction = command.Transaction;
        using var connection = command.Connection;
        var result = command.GetDataTable<TCommand, TDataAdapter>(query);

        return result;
    }

    public static DataTable GetDataTable<TCommand, TConnection, TDataAdapter>(
        this TConnection con,
        string query
    )
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
        where TDataAdapter : DbDataAdapter, new()
    {
        using var command = con.InitCommand<TCommand, TConnection>();
        using var transaction = command.Transaction;
        using var connection = command.Connection;
        var result = command.GetDataTable<TCommand, TDataAdapter>(query);

        return result;
    }

    public static DataTable GetDataTable<TCommand, TConnection>(this TConnection con, string query)
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
    {
        using var command = con.InitCommand<TCommand, TConnection>();
        using var transaction = command.Transaction;
        using var connection = command.Connection;
        var result = command.GetDataTable(query);

        return result;
    }
}
