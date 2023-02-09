using System.Reactive.Concurrency;

using ReactiveUI;

using WorkTool.Core.Modules.Common.Interfaces;
using WorkTool.Core.Modules.DependencyInjection.Interfaces;
using WorkTool.Core.Modules.Ui.Interfaces;

namespace WorkTool.Console.ViewModels;

public class CreateTimerViewModel : ViewModelBase
{
    private string name = string.Empty;
    private TimeSpan time;
    private byte seconds;

    public CreateTimerViewModel(
        IScheduler scheduler,
        IHumanizing<Exception, object> humanizing,
        IMessageBoxView messageBoxView,
        IInvoker invoker
    ) : base(scheduler, humanizing, messageBoxView, invoker) { }

    public string Name
    {
        get => name;
        set => this.RaiseAndSetIfChanged(ref name, value);
    }

    public TimeSpan Time
    {
        get => time;
        set => this.RaiseAndSetIfChanged(ref time, value);
    }

    public byte Seconds
    {
        get => seconds;
        set => this.RaiseAndSetIfChanged(ref seconds, value);
    }
}
