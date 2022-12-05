namespace WorkTool.Core.Modules.AvaloniaUi.ViewModels;

public class ViewModelBase : ReactiveObject
{
    private readonly IHumanizing<Exception, object> humanizing;
    private readonly IMessageBoxView messageBoxView;

    public ReplaySubject<bool> CanExecute { get; }

    public ViewModelBase(IHumanizing<Exception, object> humanizing, IMessageBoxView messageBoxView)
    {
        this.humanizing = humanizing.ThrowIfNull();
        this.messageBoxView = messageBoxView.ThrowIfNull();
        CanExecute = new ReplaySubject<bool>(1);
        CanExecute.OnNext(true);
    }

    public ICommand CreateCommand<TValue>(Action<TValue> action)
    {
        return ReactiveCommand.CreateFromTask(CreateAction(action), CanExecute);
    }

    public ICommand CreateCommand(Action action)
    {
        return ReactiveCommand.Create(CreateAction(action), CanExecute);
    }

    public ICommand CreateCommand(Func<Task> func)
    {
        return ReactiveCommand.Create(CreateFunc(func), CanExecute);
    }

    private Func<Task> CreateAction(Action action)
    {
        return async () =>
        {
            CanExecute.OnNext(false);

            try
            {
                action.Invoke();
            }
            catch (Exception exception)
            {
                var view = humanizing.Humanize(exception);
                await messageBoxView.ShowErrorAsync(view);
            }
            finally
            {
                CanExecute.OnNext(true);
            }
        };
    }

    private Func<TValue, Task> CreateAction<TValue>(Action<TValue> action)
    {
        return async value =>
        {
            CanExecute.OnNext(false);

            try
            {
                action.Invoke(value);
            }
            catch (Exception exception)
            {
                var view = humanizing.Humanize(exception);
                await messageBoxView.ShowErrorAsync(view);
            }
            finally
            {
                CanExecute.OnNext(true);
            }
        };
    }

    private Func<Task> CreateFunc(Func<Task> func)
    {
        return async () =>
        {
            CanExecute.OnNext(false);

            try
            {
                await func.Invoke();
            }
            catch (Exception exception)
            {
                var view = humanizing.Humanize(exception);
                await messageBoxView.ShowErrorAsync(view);
            }
            finally
            {
                CanExecute.OnNext(true);
            }
        };
    }
}
