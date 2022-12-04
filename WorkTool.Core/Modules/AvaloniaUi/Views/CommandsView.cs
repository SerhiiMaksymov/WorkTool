namespace WorkTool.Core.Modules.AvaloniaUi.Views;

public class CommandsView : ReactiveCommandsControl<ViewModelBase>, ICommandView, IContentView, ISetParameter
{
    private readonly List<ArgumentValue> argumentValues;
    private readonly IInvoker            invoker;

    public CommandsView(IInvoker invoker, UiContext avaloniaUiContext, ViewModelBase viewModel)
    {
        argumentValues = new List<ArgumentValue>();
        DataContext    = viewModel;
        this.invoker   = invoker.ThrowIfNull();

        this.WhenActivated(
            disposables =>
            {
                ViewModel.CanExecute.DisposeWith(disposables);
            });

        avaloniaUiContext.InitView(this);
    }

    public void AddCommand(CommandContext commandContext)
    {
        var command = ViewModel.CreateCommand(
            async () =>
            {
                var arguments = new List<ArgumentValue>
                {
                    new (GetType(), this)
                };

                arguments.AddRange(argumentValues);
                var result = invoker.Invoke(commandContext.Task, arguments);

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

        this.AddItem(
            new Button()
                .SetCommand(command)
                .SetContent(content));
    }

    public void AddContent(Func<object> content)
    {
        var value = content.Invoke();
        SetParameter(new ArgumentValue(value.GetType(), value));

        if (value is ISetParameter setParameter)
        {
            setParameter.SetParameter(new ArgumentValue(GetType(), value));
        }

        this.SetContent(value);
    }

    public void SetParameter(ArgumentValue argumentValue)
    {
        argumentValues.Add(argumentValue);
    }
}