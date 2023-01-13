namespace WorkTool.Core.Modules.AvaloniaUi.Views;

public class ItemsControlView : ReactiveItemsControl<ViewModelBase>, IChildrenView
{
    public ItemsControlView(UiContext avaloniaUiContext, ViewModelBase viewModel)
    {
        DataContext = viewModel;
        this.WhenActivated(_ => { });
        avaloniaUiContext.InitView(this);
    }

    public void AddChild(Func<object> item)
    {
        var content = item.Invoke();
        this.AddItem(content);
    }
}
