namespace WorkTool.Core.Modules.EntityFrameworkCore.Exceptions;

public abstract class DefaultDbContextException : Exception
{
    public DefaultDbContextException(string message, Exception innerException)
        : base(message, innerException) { }
}

public class DefaultDbContextException<
    TDefaultDbContext,
    TDbConnectionOptions,
    TDbContextOptions,
    TConnectionParameters
> : DefaultDbContextException
    where TDefaultDbContext : DefaultDbContext<
            TDbConnectionOptions,
            TDbContextOptions,
            TConnectionParameters
        >
    where TDbConnectionOptions : DbConnectionContextOptions<
            TDbContextOptions,
            TConnectionParameters
        >
    where TDbContextOptions : DbContextOptions
    where TConnectionParameters : IConnectionParameters
{
    public TDefaultDbContext Context { get; }

    public DefaultDbContextException(Exception innerException, TDefaultDbContext context)
        : base(GetMessage(context), innerException)
    {
        Context = context;
    }

    public static string GetMessage(TDefaultDbContext context)
    {
        return $"{context.GetType()} exception in {context.Connection.ConnectionParameters.GetSafeConnectionString()}.";
    }
}
