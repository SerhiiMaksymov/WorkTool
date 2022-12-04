namespace WorkTool.Core.Applications.HealthStorage.Services;

public class HealthStorageDbContext<TDbConnectionOptions, TDbContextOptions, TConnectionParameters>
    : DefaultDbContext<TDbConnectionOptions, TDbContextOptions, TConnectionParameters>
    where TDbConnectionOptions : DbConnectionContextOptions<TDbContextOptions, TConnectionParameters>
    where TDbContextOptions : DbContextOptions
    where TConnectionParameters : IConnectionParameters
{
    public HealthStorageDbContext(TDbConnectionOptions dbSqLiteDbConnectionContextOptionsContextOptions)
        : base(dbSqLiteDbConnectionContextOptionsContextOptions)
    {
    }
}