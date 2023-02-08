using Avalonia.Controls;

using WorkTool.Core.Modules.AvaloniaUi.Views;

namespace WorkTool.Console.Models;

public class ConsoleModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "e03a6f47-ba8c-45bc-bf8e-27837a41a740";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static ConsoleModule()
    {
        var register = new ReadOnlyDependencyInjectorRegister();
        register.RegisterTransient<IControl, MainView>();

        register.RegisterTransient<IEnumerable<IApplicationCommandLine>>(
            (AvaloniaUiApplicationCommandLine avaloniaUiApplicationCommandLine) =>
                new IApplicationCommandLine[] { avaloniaUiApplicationCommandLine }
        );

        register.RegisterTransient<AvaloniaUiApplicationCommandLine>(
            (AvaloniaUiApplication avaloniaUiApplication) =>
                new AvaloniaUiApplicationCommandLine(avaloniaUiApplication)
        );

        MainDependencyInjector = register.Build();
    }

    public ConsoleModule() : base(IdValue, MainDependencyInjector) { }
}
