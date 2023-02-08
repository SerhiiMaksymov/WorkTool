using System.Reactive.Concurrency;

using ReactiveUI;

using WorkTool.Core.Modules.Common.Interfaces;
using WorkTool.Core.Modules.Ui.Interfaces;

namespace WorkTool.Console.Models;

public class TimerItemViewModel : ViewModelBase
{
    private string name = string.Empty;
    private TimeSpan time;

    public TimerItemViewModel(
        IScheduler scheduler,
        IHumanizing<Exception, object> humanizing,
        IMessageBoxView messageBoxView
    ) : base(scheduler, humanizing, messageBoxView) { }

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
}
