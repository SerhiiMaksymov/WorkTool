using ReactiveUI;

namespace WorkTool.Console.ViewModels;

public class TimerItemViewModel : NotifyBase
{
    private string name = string.Empty;
    private TimeSpan time;
    private Guid id;

    public Guid Id
    {
        get => id;
        set => this.RaiseAndSetIfChanged(ref id, value);
    }

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
