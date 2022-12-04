namespace WorkTool.Core.Modules.EntityFrameworkCore.Extensions;

public static class DbConnectionContextOptionsExtension
{
    public static string GetConnectionString<TDbContextOptions, TConnectionParameters>(
    this DbConnectionContextOptions<TDbContextOptions,
        TConnectionParameters> options) where TDbContextOptions : DbContextOptions
        where TConnectionParameters : IConnectionParameters
    {
        return options.ConnectionParameters.GetConnectionString();
    }
}