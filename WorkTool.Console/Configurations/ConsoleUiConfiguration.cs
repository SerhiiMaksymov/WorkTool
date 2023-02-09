using WorkTool.Console.Views;
using WorkTool.Core.Modules.AvaloniaUi.Views;
using WorkTool.Core.Modules.Ui.Helpers;
using WorkTool.Core.Modules.Ui.Interfaces;
using WorkTool.Core.Modules.Ui.Services;

namespace WorkTool.Console.Configurations;

public readonly struct ConsoleUiConfiguration : IUiConfiguration
{
    public void Configure(UiContextBuilder builder)
    {
        var nameAddTabItemFunction = NameHelper.GetNameAddTabItemFunction<MainView, TimersView>();

        builder.AddTabItemFunction<MainView, TimersView>(() => "Timer");
        builder.AddMenuItem<MainView>(nameAddTabItemFunction, "Tools", "Timer");
    }
}
