namespace WorkTool.Core.Modules.SqlServer.Helpers;

public static class SqlServerQueries
{
    public const string GetTables = @"SELECT *
FROM INFORMATION_SCHEMA.TABLES;";
}