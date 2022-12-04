namespace WorkTool.Core.Modules.EntityFrameworkCore.Extensions;

public static class DefaultDbContextExtension
{
    public static void Execute<TDefaultDbContext,
        TDbConnectionOptions,
        TDbContextOptions,
        TConnectionParameters>(this TDefaultDbContext                           context,
                               Action<TDefaultDbContext, IDbContextTransaction> execute)
        where TDefaultDbContext : DefaultDbContext<TDbConnectionOptions, TDbContextOptions, TConnectionParameters>
        where TDbConnectionOptions : DbConnectionContextOptions<TDbContextOptions, TConnectionParameters>
        where TDbContextOptions : DbContextOptions
        where TConnectionParameters : IConnectionParameters
    {
        execute.ThrowIfNull(nameof(execute));

        try
        {
            context.Execute(execute);
        }
        catch (Exception exception)
        {
            throw new DefaultDbContextException<TDefaultDbContext,
                TDbConnectionOptions,
                TDbContextOptions,
                TConnectionParameters>(exception, context);
        }
    }

    public static async Task ExecuteAsync<TDefaultDbContext,
        TDbConnectionOptions,
        TDbContextOptions,
        TConnectionParameters>(this TDefaultDbContext        context,
                               Func<TDefaultDbContext, Task> execute)
        where TDefaultDbContext : DefaultDbContext<TDbConnectionOptions, TDbContextOptions, TConnectionParameters>
        where TDbConnectionOptions : DbConnectionContextOptions<TDbContextOptions, TConnectionParameters>
        where TDbContextOptions : DbContextOptions
        where TConnectionParameters : IConnectionParameters
    {
        execute.ThrowIfNull(nameof(execute));

        try
        {
            await context.ExecuteAsync(execute);
        }
        catch (Exception exception)
        {
            throw new DefaultDbContextException<TDefaultDbContext,
                TDbConnectionOptions,
                TDbContextOptions,
                TConnectionParameters>(exception, context);
        }
    }

    public static TResult Execute<TDefaultDbContext,
        TDbConnectionOptions,
        TDbContextOptions,
        TConnectionParameters,
        TResult>(this TDefaultDbContext                                  context,
                 Func<TDefaultDbContext, IDbContextTransaction, TResult> execute)
        where TDefaultDbContext : DefaultDbContext<TDbConnectionOptions, TDbContextOptions, TConnectionParameters>
        where TDbConnectionOptions : DbConnectionContextOptions<TDbContextOptions, TConnectionParameters>
        where TDbContextOptions : DbContextOptions
        where TConnectionParameters : IConnectionParameters
    {
        execute.ThrowIfNull(nameof(execute));

        try
        {
            return context.Execute(execute);
        }
        catch (Exception exception)
        {
            throw new DefaultDbContextException<TDefaultDbContext,
                TDbConnectionOptions,
                TDbContextOptions,
                TConnectionParameters>(exception, context);
        }
    }

    public static async Task<TResult> ExecuteAsync<TDefaultDbContext,
        TDbConnectionOptions,
        TDbContextOptions,
        TConnectionParameters,
        TResult>(this TDefaultDbContext                 context,
                 Func<TDefaultDbContext, Task<TResult>> execute)
        where TDefaultDbContext : DefaultDbContext<TDbConnectionOptions, TDbContextOptions, TConnectionParameters>
        where TDbConnectionOptions : DbConnectionContextOptions<TDbContextOptions, TConnectionParameters>
        where TDbContextOptions : DbContextOptions
        where TConnectionParameters : IConnectionParameters
    {
        try
        {
            return await context.ExecuteAsync(execute);
        }
        catch (Exception exception)
        {
            throw new DefaultDbContextException<TDefaultDbContext,
                TDbConnectionOptions,
                TDbContextOptions,
                TConnectionParameters>(exception, context);
        }
    }

    public static async Task<int> ExecuteNonQueryAsync<TDefaultDbContext,
        TDbConnectionOptions,
        TDbContextOptions,
        TConnectionParameters,
        TDbConnection,
        TCommand>(this TDefaultDbContext context,
                  string                 query)
        where TDefaultDbContext : DefaultDbContext<TDbConnectionOptions, TDbContextOptions, TConnectionParameters>
        where TDbConnectionOptions : DbConnectionContextOptions<TDbContextOptions, TConnectionParameters>
        where TDbContextOptions : DbContextOptions
        where TConnectionParameters : IConnectionParameters
        where TDbConnection : DbConnection, new()
        where TCommand : DbCommand, new()
    {
        try
        {
            query.ThrowIfNullOrWhiteSpace(nameof(query));

            return await context.Connection.ConnectionParameters
                .ExecuteNonQueryAsync<TConnectionParameters, TDbConnection, TCommand>(query);
        }
        catch (Exception exception)
        {
            throw new DefaultDbContextException<TDefaultDbContext,
                TDbConnectionOptions,
                TDbContextOptions,
                TConnectionParameters>(exception, context);
        }
    }
}