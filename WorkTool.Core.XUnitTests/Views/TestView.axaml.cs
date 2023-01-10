namespace WorkTool.Core.XUnitTests.Views;

public partial class TestView : UserControl
{
    public TestView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
