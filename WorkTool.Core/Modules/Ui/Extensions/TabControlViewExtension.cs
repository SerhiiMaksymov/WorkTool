namespace WorkTool.Core.Modules.Ui.Extensions;

public static class TabControlViewExtension
{
    public static TTabControlView AddTabItem<TTabControlView>(
        this TTabControlView tabControlView,
        Func<object> header,
        Func<object> content
    ) where TTabControlView : ITabControlView
    {
        tabControlView.AddTabItem(new TabItemContext(header, content));

        return tabControlView;
    }
}
