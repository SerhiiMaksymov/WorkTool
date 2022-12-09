namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

[TemplatePart(MenuPartName, typeof(Menu))]
[TemplatePart(TabControlPartName, typeof(TabControl))]
public class TabbedControl : ContentControl
{
    public const string MenuPartName = "PART_Menu";
    public const string TabControlPartName = "PART_TabControl";

    protected Menu? Menu;
    protected TabControl? Tabs;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        Menu = e.NameScope.Get<Menu>(MenuPartName);
        Tabs = e.NameScope.Get<TabControl>(TabControlPartName);
    }
}
