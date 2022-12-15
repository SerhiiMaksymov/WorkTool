namespace WorkTool.Core.Modules.SmsClub.Configurations;

public readonly struct UiConfiguration : IUiConfiguration
{
    public void Configure(UiContextBuilder builder)
    {
        var nameAddTabItemFunction = NameHelper.GetNameAddTabItemFunction<
            MainView,
            ControlPanelView
        >();

        builder.AddTabItemFunction<MainView, ControlPanelView>(() => "SMS Club");
        builder.AddMenuItem<MainView>(nameAddTabItemFunction, "SMS", "SMS Club");
    }
}
