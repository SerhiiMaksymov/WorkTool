namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class TabbedControl : ContentControl
{
    public readonly Menu       Menu;
    public readonly TabControl Tabs;

    public TabbedControl()
    {
        Menu = new Menu();
        Tabs = new TabControl();
    }
}