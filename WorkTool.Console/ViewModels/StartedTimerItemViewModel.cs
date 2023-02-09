using System.Reactive.Linq;

using ReactiveUI;

namespace WorkTool.Console.ViewModels;

public class StartedTimerItemViewModel : NotifyBase, IDisposable
{
    private TimerItemViewModel? timer;
    private DateTime start;

    public ObservableAsPropertyHelper<TimeSpan>? RestTimeProperty;
    public ObservableAsPropertyHelper<double>? ProgressProperty;

    public TimeSpan RestTime => RestTimeProperty?.Value ?? TimeSpan.Zero;
    public double Progress => ProgressProperty?.Value ?? 0;

    public DateTime Start
    {
        get => start;
        set => this.RaiseAndSetIfChanged(ref start, value);
    }

    public TimerItemViewModel? Timer
    {
        get => timer;
        set => this.RaiseAndSetIfChanged(ref timer, value);
    }

    public void StartTimer()
    {
        var startedDateTime = DateTime.Now;
        var time = Timer.ThrowIfNull().Time;

        Observable
            .Interval(TimeSpan.FromMilliseconds(50), RxApp.MainThreadScheduler)
            .Select(_ => DateTime.Now - startedDateTime)
            .TakeWhile(elapsed => elapsed < time)
            .Select(elapsed => time - elapsed)
            .ToProperty(this, x => x.RestTime, out RestTimeProperty);

        Observable
            .Interval(TimeSpan.FromMilliseconds(50), RxApp.MainThreadScheduler)
            .Select(_ => DateTime.Now - startedDateTime)
            .TakeWhile(elapsed => elapsed < time)
            .Select(_ => (time.TotalSeconds - RestTime.TotalSeconds) / time.TotalSeconds)
            .ToProperty(this, x => x.Progress, out ProgressProperty);
    }

    public void Dispose()
    {
        RestTimeProperty?.Dispose();
        ProgressProperty?.Dispose();
    }
}
