using System.Reactive.Concurrency;

using Avalonia.Collections;

using WorkTool.Core.Modules.Common.Interfaces;
using WorkTool.Core.Modules.Ui.Interfaces;

namespace WorkTool.Console.ViewModels;

public class TimersViewModel : ViewModelBase
{
    public TimersViewModel(
        IScheduler scheduler,
        IHumanizing<Exception, object> humanizing,
        IMessageBoxView messageBoxView
    ) : base(scheduler, humanizing, messageBoxView)
    {
        Timers = new();
    }

    public AvaloniaList<TimerItemViewModel> Timers { get; }
}
