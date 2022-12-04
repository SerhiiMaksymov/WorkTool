namespace WorkTool.Core.Modules.AdoDotNet.Extensions;

public static class DbCommandExtension
{
    public static async Task<object> ExecuteScalarAsync<TCommand>(this TCommand command, string query)
        where TCommand : DbCommand
    {
        query.ThrowIfNullOrWhiteSpace();
        command.CommandText = query;
        var result = await command.ExecuteScalarAsync();
        command.CommandText = string.Empty;

        return result;
    }

    public static async Task<object> ExecuteScalarAsync<TCommand, TParameter>(this TCommand command, string query,
                                                                              IEnumerable<TParameter> parameters)
        where TCommand : DbCommand
    {
        query.ThrowIfNullOrWhiteSpace();
        command.CommandText = query;
        command.Parameters.AddRange(parameters.ToArray());
        var result = await command.ExecuteScalarAsync();
        command.CommandText = string.Empty;
        command.Parameters.Clear();

        return result;
    }

    public static async Task<int> ExecuteNonQueryAsync<TCommand>(this TCommand command, string query)
        where TCommand : DbCommand
    {
        query.ThrowIfNullOrWhiteSpace();
        command.CommandText = query;
        var result = await command.ExecuteNonQueryAsync();
        command.CommandText = string.Empty;

        return result;
    }

    public static async Task<int> ExecuteNonQueryAsync<TCommand, TParameter>(this TCommand           command,
                                                                             string                  query,
                                                                             IEnumerable<TParameter> parameters)
        where TCommand : DbCommand
    {
        query.ThrowIfNullOrWhiteSpace();
        command.CommandText = query;
        command.Parameters.AddRange(parameters.ToArray());
        var result = await command.ExecuteNonQueryAsync();
        command.CommandText = string.Empty;
        command.Parameters.Clear();

        return result;
    }

    public static int ExecuteNonQuery<TCommand>(this TCommand command, string query)
        where TCommand : DbCommand
    {
        query.ThrowIfNullOrWhiteSpace(nameof(query));
        command.CommandText = query;
        var result = command.ExecuteNonQuery();
        command.CommandText = string.Empty;

        return result;
    }

    public static async Task<DataTable> GetDataTableAsync<TCommand>(this TCommand command, string query)
        where TCommand : DbCommand
    {
        command.ThrowIfNull();
        query.ThrowIfNullOrWhiteSpace(nameof(query));
        command.CommandText = query;
        await using var reader = await command.ExecuteReaderAsync();
        var             result = new DataTable();
        result.Load(reader);
        command.CommandText = string.Empty;

        return result;
    }

    public static DataTable GetDataTable<TCommand, TDataAdapter>(this TCommand command, string query)
        where TCommand : DbCommand where TDataAdapter : DbDataAdapter, new()
    {
        query.ThrowIfNullOrWhiteSpace();
        command.CommandText = query;
        var dataAdapter = new TDataAdapter();
        dataAdapter.SelectCommand = command;
        var result = new DataTable();
        dataAdapter.Fill(result);
        command.CommandText = string.Empty;

        return result;
    }

    public static DataTable GetDataTable<TCommand, TDataAdapter, TParameter>(this TCommand           command,
                                                                             string                  query,
                                                                             IEnumerable<TParameter> parameters)
        where TCommand : DbCommand
        where TDataAdapter : DbDataAdapter, new()
        where TParameter : DbParameter
    {
        query.ThrowIfNullOrWhiteSpace(nameof(query));
        command.CommandText = query;
        command.Parameters.AddRange(parameters.ToArray());
        var dataAdapter = new TDataAdapter();
        dataAdapter.SelectCommand = command;
        var result = new DataTable();
        dataAdapter.Fill(result);
        command.CommandText = string.Empty;

        return result;
    }

    public static Task<DataTable> GetDataTableAsync<TCommand, TDataAdapter, TParameter>(this TCommand command,
        string                                                                                        query,
        IEnumerable<TParameter>                                                                       parameters)
        where TCommand : DbCommand where TDataAdapter : DbDataAdapter, new() where TParameter : DbParameter
    {
        return Task.Run(
            () =>
            {
                query.ThrowIfNullOrWhiteSpace(nameof(query));
                command.CommandText = query;
                command.Parameters.AddRange(parameters.ToArray());
                var dataAdapter = new TDataAdapter();
                dataAdapter.SelectCommand = command;
                var result = new DataTable();
                dataAdapter.Fill(result);
                command.CommandText = string.Empty;

                return result;
            });
    }

    public static async Task<DataTable> GetDataTableAsync<TCommand, TParameter>(this TCommand command,
        string                                                                                query,
        IEnumerable<TParameter>                                                               parameters)
        where TCommand : DbCommand
        where TParameter : DbParameter
    {
        query.ThrowIfNullOrWhiteSpace(nameof(query));
        command.CommandText = query;
        command.Parameters.AddRange(parameters.ToArray());
        await using var reader = await command.ExecuteReaderAsync();
        var             result = new DataTable();
        result.Load(reader);
        command.CommandText = string.Empty;

        return result;
    }

    public static DataTable GetDataTable<TCommand>(this TCommand command)
        where TCommand : DbCommand
    {
        using var reader = command.ExecuteReader();
        var       result = new DataTable();
        result.Load(reader);
        command.CommandText = string.Empty;

        return result;
    }

    public static DataTable GetDataTable<TCommand>(this TCommand command, string query)
        where TCommand : DbCommand
    {
        query.ThrowIfNullOrWhiteSpace(nameof(query));
        command.CommandText = query;
        using var reader = command.ExecuteReader();
        var       result = new DataTable();
        result.Load(reader);
        command.CommandText = string.Empty;

        return result;
    }

    public static DataTable GetDataTable<TCommand, TParameter>(this TCommand           command,
                                                               string                  query,
                                                               IEnumerable<TParameter> parameters)
        where TCommand : DbCommand
        where TParameter : DbParameter
    {
        query.ThrowIfNullOrWhiteSpace(nameof(query));
        command.CommandText = query;
        command.Parameters.AddRange(parameters.ToArray());
        using var reader = command.ExecuteReader();
        var       result = new DataTable();
        result.Load(reader);
        command.CommandText = string.Empty;

        return result;
    }
}