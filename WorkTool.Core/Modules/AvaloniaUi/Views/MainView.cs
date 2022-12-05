namespace WorkTool.Core.Modules.AvaloniaUi.Views;

public class MainView
    : ReactiveTabbedControl<ViewModelBase>,
        IMenuView,
        ITabControlView,
        IKeyBindingView
{
    private readonly IInvoker invoker;

    public MainView(IInvoker invoker, UiContext avaloniaUiContext, ViewModelBase viewModel)
    {
        DataContext = viewModel;
        this.invoker = invoker.ThrowIfNull();

        this.WhenActivated(disposables =>
        {
            ViewModel.CanExecute.DisposeWith(disposables);

            Tabs.Bind(ItemsControl.ItemsProperty, new Binding("TabItems")).DisposeWith(disposables);
        });

        avaloniaUiContext.InitView(this);
    }

    public void AddKeyBinding(KeyboardKeyGesture keyGesture, Delegate @delegate)
    {
        var command = ViewModel.CreateCommand(async () =>
        {
            var arguments = new List<ArgumentValue>
            {
                new(GetType(), this),
                new(typeof(ITabControlView), this)
            };

            var result = invoker.Invoke(@delegate, arguments);

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
        Menu.AddItem(menuItem);
    }

    public void AddTabItem(TabItemContext tabItemContext)
    {
        var header = tabItemContext.Header.Invoke();
        var content = tabItemContext.Content.Invoke();
        var tabItem = new TabItem();

        Tabs.AddItem(
            tabItem
                .SetHeader(
                    new Grid()
                        .AddColumnDefinition(GridLength.Star)
                        .AddColumnDefinition(GridLength.Auto)
                        .AddChild(new ContentControl().SetContent(header))
                        .AddChild(
                            new Button()
                                .SetGridColumn(1)
                                .SetContent(
                                    new AvaloniaPath()
                                        .SetData(GeometryConstants.Close)
                                        .SetFill(Brushes.Black)
                                )
                                .SetCommand(ViewModel.CreateCommand(() => Tabs.RemoveItem(tabItem)))
                        )
                )
                .SetContent(content)
        );
    }

    private MenuItem AddCommand(MenuItem menuItem, TreeNode<string, MenuItemContext> node)
    {
        if (node.Value.Task is null)
        {
            return menuItem;
        }

        var command = ViewModel.CreateCommand(async () =>
        {
            var result = invoker.Invoke(
                node.Value.Task,
                new[] { new ArgumentValue(GetType(), this) }
            );

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
}
