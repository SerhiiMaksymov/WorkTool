namespace WorkTool.Core.Modules.AvaloniaUi.Views;

public class WindowPopup : Window, IStyleable
{
    public const string PartPopup = "PART_Popup";

    Type IStyleable.StyleKey => typeof(WindowPopup);
    public PopupDialogControl? Popup { get; private set; }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Popup = e.NameScope.Get<PopupDialogControl>(PartPopup);
    }
}
