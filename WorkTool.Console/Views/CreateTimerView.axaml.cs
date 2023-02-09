namespace WorkTool.Console.Views;

public partial class CreateTimerView : ReactiveUserControl<CreateTimerViewModel>
{
    public CreateTimerView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
