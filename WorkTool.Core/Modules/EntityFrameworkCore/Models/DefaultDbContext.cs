namespace WorkTool.Core.Modules.EntityFrameworkCore.Models;

public abstract class DefaultDbContext<
    TDbConnectionOptions,
    TDbContextOptions,
    TConnectionParameters
> : DbContext
    where TDbConnectionOptions : DbConnectionContextOptions<
            TDbContextOptions,
            TConnectionParameters
        >
    where TDbContextOptions : DbContextOptions
    where TConnectionParameters : IConnectionParameters
{
    public TDbConnectionOptions Connection { get; }

    public DefaultDbContext(TDbConnectionOptions connection)
        : base(connection.CreateContextOptions())
    {
        Connection = connection.ThrowIfNull();
    }

    public override int SaveChanges()
    {
        return this.SaveChangesWithTriggers(base.SaveChanges);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return this.SaveChangesWithTriggers(base.SaveChanges, acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, true, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        return this.SaveChangesWithTriggersAsync(
            base.SaveChangesAsync,
            acceptAllChangesOnSuccess,
            cancellationToken
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (Connection.EnableDetailedErrors)
        {
            optionsBuilder.EnableDetailedErrors();
        }

        if (Connection.EnableSensitiveDataLogging)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        base.OnConfiguring(optionsBuilder);
    }
}
