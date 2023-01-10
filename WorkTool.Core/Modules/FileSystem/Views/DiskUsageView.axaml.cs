namespace WorkTool.Core.Modules.FileSystem.Views;

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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Focus();
    }
}
