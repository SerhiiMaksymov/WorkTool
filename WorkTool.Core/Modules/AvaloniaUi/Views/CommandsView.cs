namespace WorkTool.Core.Modules.AvaloniaUi.Views;

public class CommandsView
    : ReactiveCommandsControl<ViewModelBase>,
        ICommandView,
        IContentView,
        ISetParameter
{
    private readonly Dictionary<Type, object> argumentValues;
    private readonly IInvoker invoker;

    public CommandsView(IInvoker invoker, UiContext avaloniaUiContext, ViewModelBase viewModel)
    {
        argumentValues = new() { { GetType(), this } };
        DataContext = viewModel;
        this.invoker = invoker.ThrowIfNull();

        this.WhenActivated(disposables =>
        {
            var currentViewModel = ViewModel.ThrowIfNull();
            currentViewModel.CanExecute.DisposeWith(disposables);
        });

        avaloniaUiContext.InitView(this);
    }

    public void AddCommand(CommandContext commandContext)
    {
        var currentViewModel = ViewModel.ThrowIfNull();
        var command = currentViewModel.CreateCommand(async () =>
        {
            var result = invoker.Invoke(commandContext.Task, argumentValues);

            if (result is null)
            {
                return;
            }

            if (result is Task task)
            {
                await task;
            }
        });

        var content = commandContext.Content.Invoke();

        this.AddItem(new Button().SetCommand(command).SetContent(content));
    }

    public void SetContent(Func<object> content)
    {
        var value = content.Invoke();
        SetParameter(value.GetType(), value);

        if (value is ISetParameter setParameter)
        {
            setParameter.SetParameter(GetType(), value);
        }

        this.SetContent(value);
    }

    public void SetParameter(Type type, object value)
    {
        argumentValues[type] = value;
    }
}
