namespace WorkTool.Core.Modules.SmsClub.Views;

public partial class ControlPanelView
    : ReactiveUserControl<ControlPanelViewModel>,
        IRefreshCommandView
{
    public ControlPanelView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public ICommand RefreshCommand => ViewModel.ThrowIfNull().RefreshCommand;
}
