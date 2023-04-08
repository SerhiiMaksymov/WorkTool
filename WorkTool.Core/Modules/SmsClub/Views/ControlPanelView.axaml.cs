namespace WorkTool.Core.Modules.SmsClub.Views;

public partial class SmsClubPanelView
    : ReactiveUserControl<ControlPanelViewModel>,
        IRefreshCommandView
{
    public SmsClubPanelView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public ICommand RefreshCommand => ViewModel.ThrowIfNull().RefreshCommand;
}
