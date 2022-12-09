namespace WorkTool.Core.Modules.SmsClub.Views;

public partial class ControlPanelView : ReactiveUserControl<ControlPanelViewModel>
{
    public ControlPanelView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
