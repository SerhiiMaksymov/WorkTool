namespace WorkTool.Console.Views;

public partial class TimersView : ReactiveUserControl<TimersViewModel>
{
    public TimersView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
