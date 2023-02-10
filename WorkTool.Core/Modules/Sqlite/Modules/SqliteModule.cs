using WorkTool.Core.Modules.Sqlite.Configurations;

namespace WorkTool.Core.Modules.Sqlite.Modules;

public class SqliteModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "8c781891-54e7-48c8-b25a-5d3f793154a2";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static SqliteModule()
    {
        var register = new DependencyInjectorRegister();
        register.RegisterConfiguration<SqliteDependencyInjectorConfiguration>();
        MainDependencyInjector = register.Build();
    }

    public SqliteModule() : base(IdValue, MainDependencyInjector) { }
}
