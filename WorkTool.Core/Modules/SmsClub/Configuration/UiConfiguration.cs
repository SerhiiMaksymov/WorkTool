namespace WorkTool.Core.Modules.SmsClub.Configuration;

public class UiConfiguration : IUiConfiguration
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
