namespace WorkTool.Core.Modules.SmsClub.Configuration;

public class UiConfiguration : IUiConfiguration
{
    public void Configure(UiContextBuilder builder)
    {
        builder.AddFunction(
            $"Add{nameof(ControlPanelView)}",
            (ITabControlView view) => view.AddTabItem(() => "Test", () => new ControlPanelView())
        );

        builder.AddMenuItem<MainView>($"Add{nameof(ControlPanelView)}", "Test", "Test");
    }
}
