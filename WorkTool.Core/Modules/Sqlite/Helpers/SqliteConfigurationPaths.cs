namespace WorkTool.Core.Modules.Sqlite.Helpers;

public static class SqliteConfigurationPaths
{
    public const string ConfigPath = "Sqlite";
    public const string ConfigConnectionStringPath = $"{ConfigPath}:ConnectionString";
    public const string ConfigEnableDetailedErrorsPath = $"{ConfigPath}:EnableDetailedErrors";
    public const string ConfigEnableSensitiveDataLoggingPath =
        $"{ConfigPath}:EnableSensitiveDataLogging";
}
