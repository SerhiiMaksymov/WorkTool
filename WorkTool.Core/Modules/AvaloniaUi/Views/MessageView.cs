namespace WorkTool.Core.Modules.AvaloniaUi.Views;

public class MessageView : ReactiveMessageControl<ViewModelBase>, ISetParameter
{
    private readonly Dictionary<TypeInformation, object> argumentValues;
    private readonly IInvoker invoker;

    public MessageView(IInvoker invoker, UiContext avaloniaUiContext, ViewModelBase viewModel)
    {
        argumentValues = new() { { GetType(), this }, { typeof(ITabControlView), this } };
        DataContext = viewModel;
        this.invoker = invoker.ThrowIfNull();
        this.WhenActivated(_ => { });
        avaloniaUiContext.InitView(this);
    }

    public ICommand CreateCommand(Delegate @delegate)
    {
        var currentViewModel = ViewModel.ThrowIfNull();

        return currentViewModel.CreateCommand(
            (Action)(
                async () =>
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
                }
            )
        );
    }

    public void SetParameter(Type type, object value)
    {
        argumentValues[type] = value;
    }
}
