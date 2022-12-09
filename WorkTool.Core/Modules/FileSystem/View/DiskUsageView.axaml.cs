namespace WorkTool.Core.Modules.FileSystem.View;

public partial class DiskUsageView : ReactiveUserControl<DiskUsageViewModel>
{
    public DiskUsageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
