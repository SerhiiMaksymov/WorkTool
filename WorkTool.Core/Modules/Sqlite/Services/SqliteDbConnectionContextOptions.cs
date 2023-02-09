using WorkTool.Core.Modules.Sqlite.Models;

namespace WorkTool.Core.Modules.Sqlite.Services;

public class SqliteDbConnectionContextOptions<TDbContextOptions>
    : DbConnectionContextOptions<TDbContextOptions, SqliteConnectionParameters>
    where TDbContextOptions : DbContextOptions
{
    public SqliteDbConnectionContextOptions(
        SqliteConnectionParameters connectionParameters,
        bool enableDetailedErrors,
        bool enableSensitiveDataLogging
    ) : base(connectionParameters, enableDetailedErrors, enableSensitiveDataLogging) { }

    public override TDbContextOptions CreateContextOptions()
    {
        var connectionString = ConnectionParameters.GetConnectionString();

        var result = (TDbContextOptions)
            new DbContextOptionsBuilder().UseSqlite(connectionString).Options;

        return result;
    }
}
