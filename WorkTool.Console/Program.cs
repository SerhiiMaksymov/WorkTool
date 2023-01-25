using WorkTool.Core;
using WorkTool.Core.Modules.Graph.Models;
using WorkTool.Core.Modules.Graph.Services;
using WorkTool.Core.Modules.ModularSystem.Interfaces;
using WorkTool.Core.Modules.ModularSystem.Services;
using WorkTool.Core.Modules.ModularSystem.Extensions;
using WorkTool.Core.Modules.ReactiveUI.Modules;
using WorkTool.Core.Modules.Ui.Modules;

namespace WorkTool.Console;

class Program
{
    private static IModule module;
    public static async Task Main(string[] args)
    {
        Init();
        var applicationCommandLine = module.GetObject<IApplicationCommandLine>();
        var arguments              = new List<string> { "Root" };
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
                .Add(new AvaloniaUiModule())
                .Add(new MaterialDesignModule())
                .Add(new CommonModule())
                .Add(new CommandLineModule())
                .Add(new UiModule())
                .Add(new ReactiveUIModule())
        );

        module = new ModuleTree(builder.Build());
    }
}
