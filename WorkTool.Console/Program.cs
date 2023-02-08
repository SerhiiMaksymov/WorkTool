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
        );

        module = new ModuleTree(builder.Build());
    }
}
