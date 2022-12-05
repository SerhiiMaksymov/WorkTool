namespace WorkTool.Core.Modules.AvaloniaUi.Views;

public class MessageView : ReactiveMessageControl<ViewModelBase>
{
    private readonly Dictionary<Type, ArgumentValue> argumentValues;
    private readonly IInvoker invoker;

    public MessageView(IInvoker invoker, UiContext avaloniaUiContext, ViewModelBase viewModel)
    {
        argumentValues = new Dictionary<Type, ArgumentValue>();
        DataContext = viewModel;
        this.invoker = invoker.ThrowIfNull();
        avaloniaUiContext.InitView(this);
    }

    public MessageView AddArgumentValue(ArgumentValue argument)
    {
        argumentValues.Add(argument.Type, argument);

        return this;
    }

    public ICommand CreateCommand(Delegate @delegate)
    {
        return ViewModel.CreateCommand(async () =>
        {
            var result = invoker.Invoke(
                @delegate,
                argumentValues
                    .Select(x => x.Value)
                    .ToList()
                    .AddItem(this.ToArgumentValue())
                    .AddItem(this.As<ITabControlView>().ToArgumentValue())
                    .ToArray()
            );

            if (result is null)
            {
                return;
            }

            if (result is Task task)
            {
                await task;
            }
        });
    }
}
