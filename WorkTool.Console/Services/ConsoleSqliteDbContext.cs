using Microsoft.EntityFrameworkCore;

using WorkTool.Core.Modules.EntityFrameworkCore.Extensions;
using WorkTool.Core.Modules.EntityFrameworkCore.Models;
using WorkTool.Core.Modules.Sqlite.Models;

namespace WorkTool.Console.Services;

public class ConsoleSqliteDbContext
    : DefaultDbContext<
        DbConnectionContextOptions<DbContextOptions, SqliteConnectionParameters>,
        DbContextOptions,
        SqliteConnectionParameters
    >
{
    public ConsoleSqliteDbContext(
        DbConnectionContextOptions<DbContextOptions, SqliteConnectionParameters> connection
    ) : base(connection) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssemblies();
        base.OnModelCreating(modelBuilder);
    }
}
