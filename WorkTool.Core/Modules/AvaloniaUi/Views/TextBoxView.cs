namespace WorkTool.Core.Modules.AvaloniaUi.Views;

public class TextBoxView : ReactiveTextBox<ViewModelBase>, IContextMenuView, ISetParameter
{
    private readonly IInvoker            invoker;
    private readonly List<ArgumentValue> parameters;

    public TextBoxView(IInvoker invoker, UiContext avaloniaUiContext, ViewModelBase viewModel)
    {
        parameters   = new List<ArgumentValue>();
        DataContext  = viewModel;
        this.invoker = invoker.ThrowIfNull();
        avaloniaUiContext.InitView(this);
    }

    public void AddMenuItem(TreeNode<string, MenuItemContext> node)
    {
        var contextFlyout = ContextFlyout as MenuFlyout;

        if (ContextFlyout is null)
        {
            contextFlyout = new MenuFlyout();

            contextFlyout.AddItem(
                    new MenuItem()
                        .SetHeader("Cut")
                        .SetCommand(ReactiveCommand.Create(Cut))
                        .BindValue(
                            IsEnabledProperty,
                            new Binding("CanCut")
                                .SetRelativeSource(
                                    new RelativeSource(RelativeSourceMode.TemplatedParent)
                                        .SetAncestorType(typeof(TextBox)))))
                .AddItem(
                    new MenuItem()
                        .SetHeader("Copy")
                        .SetCommand(ReactiveCommand.Create(Copy))
                        .BindValue(
                            IsEnabledProperty,
                            new Binding("CanCopy")
                                .SetRelativeSource(
                                    new RelativeSource(RelativeSourceMode.TemplatedParent)
                                        .SetAncestorType(typeof(TextBox)))))
                .AddItem(
                    new MenuItem()
                        .SetHeader("Paste")
                        .SetCommand(ReactiveCommand.Create(Paste))
                        .BindValue(
                            IsEnabledProperty,
                            new Binding("CanPaste")
                                .SetRelativeSource(
                                    new RelativeSource(RelativeSourceMode.TemplatedParent)
                                        .SetAncestorType(typeof(TextBox)))));
        }

        var menuItem = ToMenuItem(node);
        contextFlyout.AddItem(menuItem);
    }

    public void SetParameter(ArgumentValue argumentValue)
    {
        parameters.Add(argumentValue);
    }

    private MenuItem ToMenuItem(TreeNode<string, MenuItemContext> node)
    {
        return AddHeader(AddCommand(new MenuItem(), node), node)
            .SetItems(node.Nodes.Select(x => ToMenuItem(x)).ToArray());
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

    private MenuItem AddCommand(MenuItem menuItem, TreeNode<string, MenuItemContext> node)
    {
        if (node.Value.Task is null)
        {
            return menuItem;
        }

        var command = ViewModel.CreateCommand(
            async () =>
            {
                var arguments = new List<ArgumentValue>
                {
                    new (GetType(), this)
                };

                arguments.AddRange(parameters);
                var result = invoker.Invoke(node.Value.Task, arguments);

                if (result is Task task)
                {
                    await task;
                }
            });

        return menuItem.SetCommand(command);
    }
}