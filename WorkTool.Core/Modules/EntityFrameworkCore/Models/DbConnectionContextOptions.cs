namespace WorkTool.Core.Modules.EntityFrameworkCore.Models;

public abstract class DbConnectionContextOptions<TDbContextOptions, TConnectionParameters>
    where TDbContextOptions : DbContextOptions where TConnectionParameters : IConnectionParameters
{
    public TConnectionParameters ConnectionParameters       { get; }
    public bool                  EnableDetailedErrors       { get; }
    public bool                  EnableSensitiveDataLogging { get; }

    protected DbConnectionContextOptions(TConnectionParameters connectionParameters,
                                         bool                  enableDetailedErrors,
                                         bool                  enableSensitiveDataLogging)
    {
        ConnectionParameters       = connectionParameters.ThrowIfNull(nameof(connectionParameters));
        EnableDetailedErrors       = enableDetailedErrors;
        EnableSensitiveDataLogging = enableSensitiveDataLogging;
    }

    public abstract TDbContextOptions CreateContextOptions();
}