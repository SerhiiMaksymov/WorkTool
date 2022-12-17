namespace WorkTool.Core.Modules.FileSystem.Configurations;

public readonly struct UiConfiguration : IUiConfiguration
{
    public void Configure(UiContextBuilder builder)
    {
        var nameAddTabItemFunction = NameHelper.GetNameAddTabItemFunction<
            MainView,
            DiskUsageView
        >();

        builder.AddTabItemFunction<MainView, DiskUsageView>(() => "Disk Usage");
        builder.AddMenuItem<MainView>(nameAddTabItemFunction, "Tools", "Disk Usage");
    }
}
