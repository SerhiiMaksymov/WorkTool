namespace WorkTool.Core.Modules.AvaloniaUi.Views;

public class WrapPanelView : ReactiveWrapPanel<ViewModelBase>, IChildrenView
{
    public WrapPanelView(UiContext avaloniaUiContext, ViewModelBase viewModel)
    {
        DataContext = viewModel;
        avaloniaUiContext.InitView(this);
    }

    public void AddChild(Func<object> item)
    {
        var content = item.Invoke();
        this.AddChild(new ContentControl().SetContent(content));
    }
}