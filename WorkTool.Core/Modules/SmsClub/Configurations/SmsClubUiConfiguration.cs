namespace WorkTool.Core.Modules.SmsClub.Configurations;

public readonly struct SmsClubUiConfiguration : IUiConfiguration
{
    public void Configure(UiContextBuilder builder)
    {
        var nameAddTabItemFunction = NameHelper.GetNameAddTabItemFunction<
            MainView,
            SmsClubPanelView
        >();

        builder.AddTabItemFunction<MainView, SmsClubPanelView>(() => "SMS Club");
        builder.AddMenuItem<MainView>(nameAddTabItemFunction, "SMS", "SMS Club");
    }
}
