using WorkTool.Core.Modules.Configuration.Extensions;
using WorkTool.Core.Modules.Sqlite.Extensions;
using WorkTool.Core.Modules.Sqlite.Helpers;
using WorkTool.Core.Modules.Sqlite.Models;
using WorkTool.Core.Modules.Sqlite.Services;

namespace WorkTool.Core.Modules.Sqlite.Configurations;

public readonly struct SqliteDependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient<
            DbConnectionContextOptions<DbContextOptions, SqliteConnectionParameters>
        >((SqliteDbConnectionContextOptions<DbContextOptions> options) => options);

        register.RegisterTransient<SqliteDbConnectionContextOptions<DbContextOptions>>(
            (SqliteConnectionParameters parameters, IConfiguration configuration) =>
                new SqliteDbConnectionContextOptions<DbContextOptions>(
                    parameters,
                    configuration.GetValue(
                        SqliteConfigurationPaths.ConfigEnableDetailedErrorsPath,
                        value => bool.Parse(value),
                        false
                    ),
                    configuration.GetValue(
                        SqliteConfigurationPaths.ConfigEnableSensitiveDataLoggingPath,
                        value => bool.Parse(value),
                        false
                    )
                )
        );

        register.RegisterTransient<SqliteConnectionParameters>(
            (IConfiguration configuration) =>
                configuration[SqliteConfigurationPaths.ConfigConnectionStringPath]
                    .ThrowIfNull(SqliteConfigurationPaths.ConfigConnectionStringPath)
                    .ToSqliteConnectionParameters()
        );
    }
}
