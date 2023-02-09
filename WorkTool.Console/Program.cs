using Avalonia;

using WorkTool.Console.Modules;
using WorkTool.Core.Modules.AutoMapper.Modules;
using WorkTool.Core.Modules.Sqlite.Modules;

namespace WorkTool.Console;

class Program
{
    private static IModule? module;

    public static async Task Main(string[] args)
    {
        Init();
        var applicationCommandLine = module.ThrowIfNull().GetObject<IApplicationCommandLine>();
        var arguments = new List<string> { "Root" };
        arguments.AddRange(args);
        var argsArray = arguments.ToArray();
        await applicationCommandLine.RunAsync(argsArray);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        Init();

        return module.ThrowIfNull().GetObject<AppBuilder>();
    }

    private static void Init()
    {
        var builder = new TreeBuilder<Guid, IModule>().SetRoot(
            new TreeNodeBuilder<Guid, IModule>()
                .SetKey(ConsoleModule.IdValue)
                .SetValue(new ConsoleModule())
                .Add(new MaterialDesignModule())
                .Add(new CommonModule())
                .Add(new CommandLineModule())
                .Add(new UiModule())
                .Add(new ReactiveUIModule())
                .Add(new AvaloniaUiDesktopModule())
                .Add(new SmsClubModule())
                .Add(new FileSystemModule())
                .Add(new AvaloniaUiModule())
                .Add(new ConfigurationModule())
                .Add(new AutoMapperModule())
                .Add(new SqliteModule())
        );

        module = new ModuleTree(builder.Build());
    }
}
