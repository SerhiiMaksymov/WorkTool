namespace WorkTool.Core.Modules.AdoDotNet.Extensions;

public static class DbContextExtension
{
    public static int ExecuteNonCommand<TCommand, TParameter>(
        this DbContext context,
        string query,
        IEnumerable<TParameter> parameters
    )
        where TCommand : DbCommand, new()
        where TParameter : DbParameter
    {
        using var command = context.CreateCommand<TCommand, TParameter>(query, parameters);

        return command.ExecuteNonQuery();
    }

    public static Task<int> ExecuteNonCommandAsync<TCommand, TParameter>(
        this DbContext context,
        string query,
        IEnumerable<TParameter> parameters
    )
        where TCommand : DbCommand, new()
        where TParameter : DbParameter
    {
        using var command = context.CreateCommand<TCommand, TParameter>(query, parameters);

        return command.ExecuteNonQueryAsync();
    }

    public static int ExecuteNonCommand<TCommand>(this DbContext context, string query)
        where TCommand : DbCommand, new()
    {
        using var command = context.CreateCommand<TCommand>(query);

        return command.ExecuteNonQuery();
    }

    public static object? ExecuteScalarCommand<TCommand, TParameter>(
        this DbContext context,
        string query,
        IEnumerable<TParameter> parameters
    )
        where TCommand : DbCommand, new()
        where TParameter : DbParameter
    {
        using var command = context.CreateCommand<TCommand, TParameter>(query, parameters);

        return command.ExecuteScalar();
    }

    public static object? ExecuteScalarCommand<TCommand>(this DbContext context, string query)
        where TCommand : DbCommand, new()
    {
        using var command = context.CreateCommand<TCommand>(query);

        return command.ExecuteScalar();
    }

    public static TCommand CreateCommand<TCommand>(this DbContext context)
        where TCommand : DbCommand, new()
    {
        var connection = context.Database.GetDbConnection();

        var command = new TCommand
        {
            Connection = connection,
            Transaction = context.Database.CurrentTransaction?.GetDbTransaction()
        };

        return command;
    }

    public static TCommand CreateCommand<TCommand>(this DbContext context, string query)
        where TCommand : DbCommand, new()
    {
        var command = context.CreateCommand<TCommand>();
        command.CommandText = query;

        return command;
    }

    public static TCommand CreateCommand<TCommand, TParameter>(
        this DbContext context,
        string query,
        IEnumerable<TParameter> parameters
    )
        where TCommand : DbCommand, new()
        where TParameter : DbParameter
    {
        var command = context.CreateCommand<TCommand>(query);
        command.Parameters.AddRange(parameters.ToArray());

        return command;
    }

    public static async Task<DataTable> GetDataTablesync<TContext, TCommand, TParameter>(
        this TContext context,
        string query,
        IEnumerable<TParameter> parameters
    )
        where TCommand : DbCommand, new()
        where TParameter : DbParameter
        where TContext : DbContext
    {
        await using var command = context.CreateCommand<TCommand>();
        var result = await command.GetDataTableAsync(query, parameters);

        return result;
    }

    public static DataTable GetDataTable<TContext, TCommand, TParameter>(
        this TContext context,
        string query,
        IEnumerable<TParameter> parameters
    )
        where TCommand : DbCommand, new()
        where TParameter : DbParameter
        where TContext : DbContext
    {
        using var command = context.CreateCommand<TCommand>();
        var result = command.GetDataTable(query, parameters);

        return result;
    }

    public static async Task<DataTable> GetDataTableAsync<
        TContext,
        TCommand,
        TDataAdapter,
        TParameter
    >(this TContext context, string query, IEnumerable<TParameter> parameters)
        where TCommand : DbCommand, new()
        where TParameter : DbParameter
        where TContext : DbContext
        where TDataAdapter : DbDataAdapter, new()
    {
        await using var command = context.CreateCommand<TCommand>();
        var result = await command.GetDataTableAsync<TCommand, TDataAdapter, TParameter>(
            query,
            parameters
        );

        return result;
    }

    public static DataTable GetDataTable<TContext, TCommand>(this TContext context, string query)
        where TCommand : DbCommand, new()
        where TContext : DbContext
    {
        using var command = context.CreateCommand<TCommand>();
        var result = command.GetDataTable(query);

        return result;
    }

    public static async Task<DataTable> GetDataTableAsync<TContext, TCommand>(
        this TContext context,
        string query
    )
        where TCommand : DbCommand, new()
        where TContext : DbContext
    {
        await using var command = context.CreateCommand<TCommand>();
        var result = await command.GetDataTableAsync(query);

        return result;
    }

    public static int ExecuteNonQuery<TContext, TCommand>(this TContext context, string query)
        where TCommand : DbCommand, new()
        where TContext : DbContext
    {
        using var command = context.CreateCommand<TCommand>();
        var result = command.ExecuteNonQuery(query);

        return result;
    }

    public static DataTable GetDataTable<TCommand>(this DbContext context, string query)
        where TCommand : DbCommand, new()
    {
        using var command = context.CreateCommand<TCommand>();
        var result = command.GetDataTable(query);

        return result;
    }

    public static DataTable GetDataTable<TCommand, TParameter>(
        this DbContext context,
        string query,
        IEnumerable<TParameter> parameters
    )
        where TParameter : DbParameter
        where TCommand : DbCommand, new()
    {
        using var command = context.CreateCommand<TCommand>();
        var result = command.GetDataTable(query, parameters);

        return result;
    }

    public static void Execute<TContext>(
        this TContext context,
        Action<TContext, IDbContextTransaction> execute
    ) where TContext : DbContext
    {
        execute.ThrowIfNull();
        var transaction = context.Database.CurrentTransaction;

        try
        {
            transaction ??= context.Database.BeginTransaction();
            execute.Invoke(context, transaction);
            transaction.Commit();
        }
        catch
        {
            transaction?.Rollback();

            throw;
        }
    }

    public static async Task ExecuteAsync<TContext>(
        this TContext context,
        Func<TContext, Task> execute
    ) where TContext : DbContext
    {
        execute.ThrowIfNull();
        var database = context.Database;
        var transaction = database.CurrentTransaction;

        try
        {
            transaction ??= await database.BeginTransactionAsync();
            await execute.Invoke(context);
            await transaction.CommitAsync();
        }
        catch
        {
            if (transaction is not null)
            {
                await transaction.RollbackAsync();
            }

            throw;
        }
    }

    public static TResult Execute<TContext, TResult>(
        this TContext context,
        Func<TContext, IDbContextTransaction, TResult> execute
    ) where TContext : DbContext
    {
        execute.ThrowIfNull();
        var transaction = context.Database.CurrentTransaction;

        try
        {
            transaction ??= context.Database.BeginTransaction();
            var result = execute.Invoke(context, transaction);
            transaction.Commit();

            return result;
        }
        catch
        {
            transaction?.Rollback();

            throw;
        }
    }

    public static async Task<TResult> ExecuteAsync<TContext, TResult>(
        this TContext context,
        Func<TContext, Task<TResult>> execute
    ) where TContext : DbContext
    {
        execute.ThrowIfNull();
        var transaction = context.Database.CurrentTransaction;

        try
        {
            transaction ??= await context.Database.BeginTransactionAsync();
            var result = await execute.Invoke(context);
            await transaction.CommitAsync();

            return result;
        }
        catch
        {
            if (transaction is not null)
            {
                await transaction.RollbackAsync();
            }

            throw;
        }
    }
}
