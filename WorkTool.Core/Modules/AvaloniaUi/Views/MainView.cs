namespace WorkTool.Core.Modules.AvaloniaUi.Views;

public class MainView
    : ReactiveTabbedControl<ViewModelBase>,
        IMenuView,
        ITabControlView,
        IKeyBindingView
{
    private readonly IInvoker invoker;
    private readonly UiContext uiContext;
    private readonly Dictionary<Type, object> argumentValues;

    public MainView(IInvoker invoker, UiContext uiContext, ViewModelBase viewModel)
    {
        DataContext = viewModel;
        this.invoker = invoker.ThrowIfNull();
        this.uiContext = uiContext;

        argumentValues = new() { { GetType(), this }, { typeof(ITabControlView), this } };

        this.WhenActivated(disposables =>
        {
            var currentViewModel = ViewModel.ThrowIfNull();
            currentViewModel.CanExecute.DisposeWith(disposables);
        });
    }

    public void AddKeyBinding(KeyboardKeyGesture keyGesture, Delegate @delegate)
    {
        var currentViewModel = ViewModel.ThrowIfNull();

        var command = currentViewModel.CreateCommand(async () =>
        {
            var result = invoker.Invoke(@delegate, argumentValues);

            if (result is null)
            {
                return;
            }

            if (result is Task task)
            {
                await task;
            }
        });

        this.AddKeyBinding(
            new KeyBinding().SetGesture(keyGesture.ToKeyGesture()).SetCommand(command)
        );
    }

    public void AddMenuItem(TreeNode<string, MenuItemContext> node)
    {
        var menuItem = ToMenuItem(node);
        Menu.ThrowIfNull().AddItem(menuItem);
    }

    public void AddTabItem(TabItemContext tabItemContext)
    {
        var currentViewModel = ViewModel.ThrowIfNull();
        var header = tabItemContext.Header.Invoke();
        var content = tabItemContext.Content.Invoke();
        var tabItem = new TabItem();

        var grid = new Grid()
            .AddColumnDefinitions(GridLength.Star, GridLength.Auto, GridLength.Auto)
            .AddChild(new ContentControl().SetContent(header))
            .AddChild(
                new ButtonMaterialIcon()
                    .SetGridColumn(2)
                    .SetKindClose()
                    .SetCommand(
                        currentViewModel.CreateCommand(() => Tabs.ThrowIfNull().RemoveItem(tabItem))
                    )
            );

        if (content is IRefreshCommandView refreshCommandView)
        {
            grid.AddChild(
                new ButtonMaterialIcon()
                    .SetGridColumn(1)
                    .SetKindRefresh()
                    .SetCommand(refreshCommandView.RefreshCommand)
            );
        }

        Tabs.ThrowIfNull().AddItem(tabItem.SetHeader(grid).SetContent(content));
    }

    private MenuItem AddCommand(MenuItem menuItem, TreeNode<string, MenuItemContext> node)
    {
        var currentViewModel = ViewModel.ThrowIfNull();
        var command = currentViewModel.CreateCommand(async () =>
        {
            var @delegate = node.Value.Task.ThrowIfNull();
            var result = invoker.Invoke(@delegate, argumentValues);

            if (result is Task task)
            {
                await task;
            }
        });

        return menuItem.SetCommand(command);
    }

    private MenuItem AddHeader(MenuItem menuItem, TreeNode<string, MenuItemContext> node)
    {
        switch (node.Value)
        {
            default:
            {
                menuItem.SetHeader(node.Value.Header.Invoke());

                break;
            }
        }

        return menuItem;
    }

    private MenuItem ToMenuItem(TreeNode<string, MenuItemContext> node)
    {
        return AddHeader(AddCommand(new MenuItem(), node), node)
            .SetItems(node.Nodes.Select(x => ToMenuItem(x)).ToArray());
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        uiContext.InitView(this);
    }
}
